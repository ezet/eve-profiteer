﻿using Caliburn.Micro;
using eZet.EveLib.Modules.Models;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Repository;
using eZet.EveProfiteer.Util;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace eZet.EveProfiteer.Services {
using OrderType = EveLib.Modules.OrderType;
    public class MarketBrowserService : DbContextService {
        private readonly EveMarketService _eveMarketService;
        private readonly EveStaticDataRepository _staticData;


        public MarketBrowserService(EveStaticDataRepository staticData, EveMarketService eveMarketService) {
            _staticData = staticData;
            _eveMarketService = eveMarketService;
        }

        public async Task<List<MapRegion>> GetRegions() {
            return await _staticData.GetRegionsOrdered().ToListAsync();
        }

        public Task<List<InvType>> GetMarketTypes() {
            return _staticData.GetMarketTypes().ToListAsync();
        }

        public async Task<BindableCollection<MarketTreeNode>> GetMarketTree() {
            var rootList = new BindableCollection<MarketTreeNode>();
            List<InvType> items = await _staticData.GetMarketTypes().ToListAsync().ConfigureAwait(false);
            List<InvMarketGroup> groupList = await _staticData.GetMarketGroups().ToListAsync().ConfigureAwait(false);
            ILookup<int, MarketTreeNode> groups = groupList.Select(t => new MarketTreeNode(t)).ToLookup(t => t.Id);
            foreach (InvType item in items) {
                var node = new MarketTreeNode(item);
                int id = item.MarketGroupId.GetValueOrDefault();
                var group = groups[id].Single();
                if (group != null) {
                    group.Children.Add(node);
                    node.Parent = group;
                }
            }
            foreach (var key in groupList) {
                var node = groups[key.MarketGroupId].Single();
                if (key.ParentGroupId.HasValue) {
                    var parent = groups[key.ParentGroupId.GetValueOrDefault()].Single();
                    parent.Children.Add(node);
                    node.Parent = parent;
                } else {
                    rootList.Add(node);
                }
            }
            return rootList;
        }

        public async Task<MarketBrowserItem> GetMarketDetails(MapRegion region, InvType invType) {
            EveMarketDataResponse<EmdItemOrders> orderResponse = await _eveMarketService.GetItemOrderAsync(region.RegionId, invType.TypeId).ConfigureAwait(false);
            var buyOrders = new List<MarketBrowserOrder>();
            var sellOrders = new List<MarketBrowserOrder>();
            foreach (EmdItemOrders.ItemOrderEntry order in orderResponse.Result.Orders) {
                MarketBrowserOrder marketBrowserOrder = ApiEntityMapper.Map(order, new MarketBrowserOrder());
                if (order.OrderType == OrderType.Buy) {
                    buyOrders.Add(marketBrowserOrder);
                } else {
                    sellOrders.Add(marketBrowserOrder);
                }
            }
            CrestMarketHistory history = await _eveMarketService.GetMarketHistoryAsync(region.RegionId, invType.TypeId).ConfigureAwait(false);
            var marketHistory = history.Entries.Select(entry => ApiEntityMapper.Map(entry, new MarketHistoryAggregateEntry())).ToList();

            var item = new MarketBrowserItem(invType, marketHistory, sellOrders, buyOrders, 7);
            return item;
        }
    }
}
