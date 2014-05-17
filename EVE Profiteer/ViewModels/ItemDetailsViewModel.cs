﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using eZet.EveOnlineDbModels;
using eZet.EveProfiteer.Events;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Services;
using MoreLinq;

namespace eZet.EveProfiteer.ViewModels {
    public class ItemDetailsViewModel : Screen, IHandle<OrdersAddedEvent> {
        private readonly IEventAggregator _eventAggregator;
        private readonly AnalyzerService _analyzerService;
        private readonly EveOnlineStaticDataService _eveDbService;
        private InvType _selectedItem;
        private ItemDetailsData _itemData;

        public ItemDetailsViewModel(IEventAggregator eventAggregator, AnalyzerService analyzerService, EveOnlineStaticDataService eveDbService) {
            _eventAggregator = eventAggregator;
            _analyzerService = analyzerService;
            _eveDbService = eveDbService;
            DisplayName = "Item Details";
            eventAggregator.Subscribe(this);
            PropertyChanged += OnPropertyChanged;
            LoadSelectableItems();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (propertyChangedEventArgs.PropertyName == "SelectedItem")
                LoadItem(SelectedItem);
        }

        private void LoadItem(InvType type) {
            List<Transaction> transactions = _analyzerService.Transactions().Where(f => f.TypeId == type.TypeId).ToList();
            ItemData = new ItemDetailsData(transactions.First().TypeId, transactions.First().TypeName, transactions);
        }

        public ItemDetailsData ItemData {
            get { return _itemData; }
            set {
                if (Equals(value, _itemData)) return;
                _itemData = value;
                NotifyOfPropertyChange(() => ItemData);
            }
        }

        public ICollection<InvType> SelectableItems { get; private set; }

        public InvType SelectedItem {
            get { return _selectedItem; }
            set {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }


        public void Handle(OrdersAddedEvent message) {
            LoadSelectableItems();
        }

        private void LoadSelectableItems() {
            IEnumerable<int> types =
                _analyzerService.Orders().DistinctBy(order => order.TypeId).Select(order => order.TypeId);
            SelectableItems = _eveDbService.GetTypes().Where(type => types.Contains(type.TypeId)).ToList();
        }
    }
}