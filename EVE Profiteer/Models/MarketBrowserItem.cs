﻿using System.Collections.Generic;
using System.Linq;

namespace eZet.EveProfiteer.Models {
    public class MarketBrowserItem {
        public MarketBrowserItem(InvType invType, IEnumerable<MarketHistoryEntry> marketHistory,
            IEnumerable<MarketOrder> sellOrders, IEnumerable<MarketOrder> buyOrders, int donchianLength) {
            InvType = invType;
            MarketHistory = marketHistory.OrderBy(entry => entry.Date).ToList();
            BuyOrders = buyOrders.OrderByDescending(t => t.Price).ToList();
            SellOrders = sellOrders.OrderBy(t => t.Price).ToList();
            DonchianLength = donchianLength;
            initialize();
        }

        public int DonchianLength { get; set; }

        public double CommodityChannelIndexFactor = 0.015;

        public InvType InvType { get; set; }
        public ICollection<MarketHistoryEntry> MarketHistory { get; set; }

        public ICollection<MarketOrder> SellOrders { get; private set; }

        public ICollection<MarketOrder> BuyOrders { get; private set; }

        private void initialize() {
            var high = new List<decimal>();
            var low = new List<decimal>();
            foreach (MarketHistoryEntry entry in MarketHistory) {
                high.Add(entry.HighPrice);
                low.Add(entry.LowPrice);
                if (high.Count > DonchianLength)
                    high.RemoveAt(0);
                if (low.Count > DonchianLength)
                    low.RemoveAt(0);
                if (high.Any())
                    entry.DonchianHigh = high.Max();
                if (low.Any())
                    entry.DonchianLow = low.Min();
                entry.DonchianCenter = (entry.DonchianHigh + entry.DonchianLow)/2;
            }
        }

        private void calcCCI(MarketHistoryEntry entry) {

            
        }
    }
}