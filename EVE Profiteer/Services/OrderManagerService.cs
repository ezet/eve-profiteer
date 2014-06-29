﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.XtraPrinting.Native;
using eZet.EveLib.Modules.Models;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Util;
using OrderType = eZet.EveLib.Modules.OrderType;

namespace eZet.EveProfiteer.Services {
    public class OrderManagerService {
        private readonly EveMarketService _eveMarketService;
        private readonly Repository _repository;

        private readonly TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);

        public OrderManagerService(EveMarketService eveMarketService, Repository repository) {
            _eveMarketService = eveMarketService;
            _repository = repository;
        }

        public Task<List<InvType>> GetMarketTypesAsync() {
            return _repository.GetMarketTypes().ToListAsync();
        }

        public async Task<List<OrderVm>> GetOrdersAsync() {
            var list = new List<OrderVm>();
            var orders = await _repository.MyOrders().Include(o => o.InvType.Assets).ToListAsync().ConfigureAwait(false);
            //var selldates = new Dictionary<int, DateTime>();
            //MyTransactions(db).Where(t => t.TransactionType == TransactionType.Sell)
            //    .GroupBy(t => t.TypeId)
            //    .Apply(grouping =>
            //        selldates.Add(grouping.Key, grouping.Max(t => t.TransactionDate)));

            //var buyDates = new Dictionary<int, DateTime>();
            //MyTransactions(db).Where(t => t.TransactionType == TransactionType.Buy)
            //    .GroupBy(t => t.TypeId)
            //    .Apply(grouping =>
            //        buyDates.Add(grouping.Key,
            //            grouping.Max(t => t.TransactionDate)));

            var marketOrders = await _repository.MyMarketOrders().Where(t => t.OrderState == OrderState.Open).ToListAsync().ConfigureAwait(false);
            var marketLookup = marketOrders.ToLookup(t => t.TypeId);
            foreach (var order in orders) {
                var ordervm = new OrderVm(order);

                //if (order.InvType.Transactions.Any()) {
                //    var date = order.InvType.Transactions.Max(t => t.TransactionDate);
                //    ordervm.LastTransaction = date;
                //}

                ordervm.HasActiveBuyOrder = marketLookup[order.TypeId].Any(t => t.Bid);
                ordervm.HasActiveSellOrder = marketLookup[order.TypeId].Any(t => !t.Bid);

                //DateTime t;
                //selldates.TryGetValue(order.TypeId, out t);
                //ordervm.LastSellDate = t;
                //ordervm.LastTransaction = ordervm.LastSellDate;

                //buyDates.TryGetValue(order.TypeId, out t);
                //ordervm.LastBuyDate = t;
                //if (ordervm.LastBuyDate > ordervm.LastSellDate) {
                //    ordervm.LastTransaction = ordervm.LastBuyDate;
                //}
                list.Add(ordervm);
            }
            return list;
        }

        public async Task<int> RemoveOrdersAsync(IEnumerable<OrderVm> orders) {
            var list = orders.Select(order => order.Order);
            var db = _repository.Db;
            foreach (var order in list) {
                if (order.Id == 0) {
                    continue;
                }
                db.Orders.Attach(order);
                db.Orders.Remove(order);
            }
            return await db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> SaveOrdersAsync(IEnumerable<OrderVm> orders) {
            var db = _repository.Db;
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            int c = 0;
            var list = orders.Select(f => f.Order).ToList();
            foreach (var order in list) {
                _trace.TraceEvent(TraceEventType.Verbose, 0, "" + c++);
                if (order.Id == 0) {
                    db.InvTypes.Attach(order.InvType);
                    await db.Entry(order.InvType).Collection(f => f.Assets).LoadAsync().ConfigureAwait(false);
                    order.ApiKeyEntity_Id = ApplicationHelper.ActiveKeyEntity.Id;
                    db.Orders.Add(order);
                } else {
                    //order.InvType = null;
                    db.Orders.Attach(order);
                    db.Entry(order).State = EntityState.Modified;
                }
            }
            db.ChangeTracker.DetectChanges();
            return await db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task LoadMarketDataAsync(IEnumerable<Order> orders, int dayLimit) {
            var enumerable = orders as IList<Order> ?? orders.ToList();
            var pricesTask =
                _eveMarketService.GetItemPricesAsync(Properties.Settings.Default.DefaultStationId,
                    enumerable.Select(o => o.TypeId)).ConfigureAwait(false);
            var historyTask = _eveMarketService.GetItemHistoryAsync(Properties.Settings.Default.DefaultRegionId,
                enumerable.Select(o => o.TypeId), dayLimit);
            var prices = await pricesTask;
            var priceLookup = prices.Prices.ToLookup(f => f.TypeId);
            var history = await historyTask;
            ILookup<int, EmdItemHistory.ItemHistoryEntry> historyLookup = history.History.ToLookup(f => f.TypeId);
            foreach (Order order in enumerable) {
                var itemHistory = historyLookup[order.TypeId].ToList();
                var price = priceLookup[order.TypeId];
                if (!itemHistory.IsEmpty()) {
                    order.AvgPrice = itemHistory.Average(f => f.AvgPrice);
                    order.AvgVolume = itemHistory.Average(f => f.Volume);
                    order.CurrentBuyPrice = price.Single(t => t.OrderType == OrderType.Buy).Price;
                    order.CurrentSellPrice = price.Single(t => t.OrderType == OrderType.Sell).Price;
                }
            }
        }
    }
}