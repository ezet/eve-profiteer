﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraPrinting.Native;
using eZet.Eve.OrderIoHelper;
using eZet.Eve.OrderIoHelper.Models;
using eZet.EveLib.Modules;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Repository;

namespace eZet.EveProfiteer.Services {
    public class OrderEditorService {

        public string BuyOrdersFileName = "BuyOrders.xml";

        public string SellOrdersFileName = "SellOrders.xml";

        private readonly IRepository<Order> _ordersRepository;


        public OrderEditorService(IRepository<Order> ordersRepository) {
            _ordersRepository = ordersRepository;
            EveMarketData = new EveMarketData();
        }

        private EveMarketData EveMarketData { get; set; }

        public ICollection<Order> LoadOrdersFromDisk(string path) {
            var orders = new List<Order>();
            try {
                var buyOrders =
                    OrderInstallerIoService.ReadBuyOrders(path + Path.DirectorySeparatorChar + BuyOrdersFileName);
                var sellOrders =
                    OrderInstallerIoService.ReadSellOrders(path + Path.DirectorySeparatorChar + SellOrdersFileName);
                var sellOrderLookup = sellOrders.ToLookup(f => f.ItemId);
                foreach (var buyOrder in buyOrders) {
                    var sellOrder = sellOrderLookup[buyOrder.ItemId].SingleOrDefault();
                    sellOrders.Remove(sellOrder);
                    orders.Add(new Order(buyOrder, sellOrder));
                }
                foreach (var sellOrder in sellOrders) {
                    orders.Add(new Order(null, sellOrder));
                }
            } catch (FileNotFoundException e) {

            }
            return orders;
        }

        public IQueryable<Order> GetOrders() {
            return _ordersRepository.All();
        }

        public void AddOrders(IEnumerable<Order> orders) {
            _ordersRepository.AddRange(orders);
        }

        public void DeleteOrders(IEnumerable<Order> orders) {
            _ordersRepository.RemoveRange(orders);
        }

        public void SaveChanges() {
            _ordersRepository.SaveChanges();
        }

        public void LoadPriceData(ICollection<Order> orders, int dayLimit) {
            if (orders.Count == 0) return;
            var historyOptions = new EveMarketDataOptions();
            foreach (var order in orders) {
                historyOptions.Items.Add(order.ItemId);
            }
            historyOptions.AgeSpan = TimeSpan.FromDays(dayLimit);
            historyOptions.Regions.Add(10000002);
            var history = EveMarketData.GetItemHistory(historyOptions);
            var historyLookup = history.Result.History.ToLookup(f => f.TypeId);
            foreach (var order in orders) {
                var itemHistory = historyLookup[order.ItemId].ToList();
                if (!itemHistory.IsEmpty()) {
                    order.AvgPrice = itemHistory.Average(f => f.AvgPrice);
                    order.AvgVolume = itemHistory.Average(f => f.Volume);
                }
            }
        }

        public void SaveOrdersToDisk(string path, ICollection<Order> orders) {
            OrderInstallerIoService.WriteBuyOrders(path + Path.DirectorySeparatorChar + BuyOrdersFileName, ToBuyOrderCollection(orders));
            OrderInstallerIoService.WriteSellOrders(path + Path.DirectorySeparatorChar + SellOrdersFileName, ToSellOrderCollection(orders));
        }

        private static BuyOrderCollection ToBuyOrderCollection(IEnumerable<Order> orders) {
            var buyOrders = new BuyOrderCollection();
            foreach (var order in orders.Where(order => order.BuyQuantity > 0)) {
                buyOrders.Add(order.ToBuyOrder());
            }
            return buyOrders;
        }

        private static SellOrderCollection ToSellOrderCollection(IEnumerable<Order> orders) {
            var sellOrders = new SellOrderCollection();
            foreach (var order in orders.Where(order => order.MinSellQuantity > 0)) {
                sellOrders.Add(order.ToSellOrder());
            }
            return sellOrders;
        }

    }


}
