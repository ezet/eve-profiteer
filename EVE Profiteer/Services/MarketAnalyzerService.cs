﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using eZet.EveLib.Modules;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Util;

namespace eZet.EveProfiteer.Services {
    public class MarketAnalyzerService : DbContextService {
        private readonly EveMarketService _eveMarketService;

        public MarketAnalyzerService(EveMarketService eveMarketService) {
            _eveMarketService = eveMarketService;
        }

        public async Task<BindableCollection<MarketTreeNode>> GetMarketTreeAsync(
            PropertyChangedEventHandler itemPropertyChanged) {
            using (var db = CreateDb()) {
                var rootList = new BindableCollection<MarketTreeNode>();
                List<InvType> items = await GetMarketTypes(db).ToListAsync();
                List<InvMarketGroup> groupList = await db.InvMarketGroups.ToListAsync();
                ILookup<int, MarketTreeNode> groups = groupList.Select(t => new MarketTreeNode(t)).ToLookup(t => t.Id);

                foreach (InvType item in items) {
                    var node = new MarketTreeNode(item);
                    int id = item.MarketGroupId.GetValueOrDefault();
                    var group = groups[id].Single();
                    if (group != null) {
                        group.Children.Add(node);
                        node.Parent = group;
                    }
                    node.PropertyChanged += itemPropertyChanged;
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
        }

        public async Task<List<MapRegion>> GetRegionsAsync() {
            using (var db = CreateDb()) {
                return await db.MapRegions.AsNoTracking().Include("StaStations").OrderBy(region => region.RegionName).ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<InvType>> GetInvTypesForOrdersAsync() {
            using (var db = CreateDb()) {
                return await MyOrders(db).Select(order => order.InvType).ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<IList<MarketAnalyzerEntry>> AnalyzeAsync(MapRegion region, StaStation station, IEnumerable<InvType> invTypes, int days) {
            var items = invTypes.Select(type => type.TypeId).ToList();
            var priceResult = await _eveMarketService.GetItemPricesAsync(station.StationId, items).ConfigureAwait(false);
            var historyResult = await _eveMarketService.GetItemHistoryAsync(region.RegionId, items, days).ConfigureAwait(false);
            var analyzer = new MarketAnalyzer(invTypes, priceResult.Prices.Where(o => o.OrderType == OrderType.Sell),
                priceResult.Prices.Where(o => o.OrderType == OrderType.Buy), historyResult.History);
            analyzer.Analyze();
            return analyzer.Result;
        }

    }
}
