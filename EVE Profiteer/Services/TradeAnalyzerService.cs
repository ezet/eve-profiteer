﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using eZet.EveProfiteer.Models;

namespace eZet.EveProfiteer.Services {
    public class TradeAnalyzerService {
        private readonly EveProfiteerRepository _eveProfiteerRepository;

        public TradeAnalyzerService(EveProfiteerRepository eveProfiteerRepository) {
            _eveProfiteerRepository = eveProfiteerRepository;
        }

        public Task<List<Transaction>> GetTransactions(DateTime start, DateTime end) {
            return _eveProfiteerRepository.MyTransactions().Include("InvType").Where(t => t.TransactionDate >= start.Date && t.TransactionDate <= end.Date).ToListAsync();
        }

        public Task<List<Order>> GetOrders() {
            return _eveProfiteerRepository.MyOrders().ToListAsync();
        }
    }
}
