﻿using System;
using System.Collections.Generic;
using System.Linq;
using eZet.EveLib.EveOnline;
using eZet.EveLib.EveOnline.Model.Character;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Util;

namespace eZet.EveProfiteer.Services {

    public class EveApiService {

        public IList<ApiKeyEntity> GetApiKeyEntities(Models.ApiKey key) {
            var ckey = new CharacterKey(key.ApiKeyId, key.VCode);
            var list = new List<ApiKeyEntity>();
            foreach (var c in ckey.Characters) {
                list.Add(new ApiKeyEntity { EntityId = c.CharacterId, Name = c.CharacterName, Type = "Character" });
            }
            return list;
        }

        public IList<Transaction> GetNewTransactions(Models.ApiKey key, ApiKeyEntity entity,
            Func<Transaction> transactionFactory, long latestId) {
            return getTransactions(key, entity, transactionFactory, 100, latestId);
        }

        public IList<Transaction> GetAllTransactions(Models.ApiKey key, ApiKeyEntity entity,
            Func<Transaction> transactionFactory) {
            return getTransactions(key, entity, transactionFactory, 5000);
        }

        public IList<JournalEntry> GetNewJournalEntries(Models.ApiKey key, ApiKeyEntity entity, Func<JournalEntry> journalFactory, long latestId) {
            return getJournalEntries(key, entity, journalFactory, 100, latestId);
        }

        public IList<JournalEntry> GetAllJournalEntries(Models.ApiKey key, ApiKeyEntity entity,
         Func<JournalEntry> transactionFactory) {
            return getJournalEntries(key, entity, transactionFactory, 5000);
        }

        private static IList<JournalEntry> getJournalEntries(Models.ApiKey key, ApiKeyEntity entity, Func<JournalEntry> factory, int rowLimit, long latestId = 0) {
            var list = new List<JournalEntry>();
            var ckey = new CharacterKey(key.ApiKeyId, key.VCode);
            var res = ckey.Characters.Single(c => c.CharacterId == entity.EntityId).GetWalletJournal(rowLimit);
            var transactions = res.Result.Journal.Where(f => f.RefId > latestId);
            var enumerable = transactions as IList<WalletJournal.JournalEntry> ?? transactions.ToList();
            int count;
            do {
                count = res.Result.Journal.Count();
                foreach (var t in enumerable) {
                    var transaction = factory.Invoke();
                    transaction.ApiKeyEntity = entity;
                    list.Add(Mapper.Map(t, transaction));
                }
                res = res.Result.GetOlder(rowLimit);
            } while (res.Result.Journal.Count() != 0 && enumerable.Count() == count);
            return list;

        }

        private static IList<Transaction> getTransactions(Models.ApiKey key, ApiKeyEntity entity,
            Func<Transaction> transactionFactory, int rowLimit, long limitId = 0) {
            var list = new List<Transaction>();
            var ckey = new CharacterKey(key.ApiKeyId, key.VCode);
            var res = ckey.Characters.Single(c => c.CharacterId == entity.EntityId).GetWalletTransactions(rowLimit);
            var transactions = res.Result.Transactions.Where(f => f.TransactionId > limitId);
            var enumerable = transactions as IList<WalletTransactions.Transaction> ?? transactions.ToList();
            int count;
            do {
                count = res.Result.Transactions.Count();
                foreach (var t in enumerable) {
                    var transaction = transactionFactory.Invoke();
                    transaction.ApiKeyEntity = entity;
                    list.Add(Mapper.Map(t, transaction));
                }
                res = res.Result.GetOlder(rowLimit);
            } while (res.Result.Transactions.Count() != 0 && enumerable.Count() == count);
            return list;
        }
    }
}