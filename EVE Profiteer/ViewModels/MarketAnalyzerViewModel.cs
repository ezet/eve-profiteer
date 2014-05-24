﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Xpf.Mvvm;
using eZet.Eve.EveProfiteer.ViewModels;
using eZet.EveProfiteer.Events;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Services;
using eZet.EveProfiteer.Util;
using Xceed.Wpf.Toolkit;

namespace eZet.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel : Screen, IHandle<OrdersAddedEventArgs> {
        private readonly EveProfiteerDataService _dataService;
        private readonly EveMarketService _eveMarketService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private int _dayLimit = 5;
        private BindableCollection<MarketAnalyzerEntry> _marketAnalyzerResults;
        private BindableCollection<InvType> _selectedItems;
        private MapRegion _selectedRegion;
        private StaStation _selectedStation;

        public MarketAnalyzerViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            EveProfiteerDataService dataService,
            EveMarketService eveMarketService) {
            _eveMarketService = eveMarketService;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _eventAggregator.Subscribe(this);
            DisplayName = "Market Analyzer";


            SelectedItems = new BindableCollection<InvType>();

            AnalyzeCommand = new DelegateCommand(Analyze, () => SelectedItems.Count != 0);
            AddToOrdersCommand = new DelegateCommand<ICollection<object>>(AddToOrders, CanAddToOrders);
            LoadOrdersCommand = new DelegateCommand(LoadOrders);
            ViewTradeDetailsCommand =
                new DelegateCommand<MarketAnalyzerEntry>(
                    entry => _eventAggregator.Publish(new ViewTradeDetailsEventArgs(entry.InvType)),
                    entry => entry != null && entry.Order != null);
            ViewMarketDetailsCommand = new DelegateCommand<MarketAnalyzerEntry>(item => _eventAggregator.Publish(new ViewMarketDetailsEventArgs(item.InvType)), item => item != null);
        }

        public ICommand ViewMarketDetailsCommand { get; private set; }


        public ICommand AddToOrdersCommand { get; private set; }

        public ICommand AnalyzeCommand { get; private set; }

        public ICommand LoadOrdersCommand { get; private set; }

        public ICommand ViewTradeDetailsCommand { get; private set; }

        public BindableCollection<InvMarketGroup> TreeRootNodes { get; private set; }

        public ICollection<MapRegion> Regions { get; private set; }

        public ICollection<StaStation> Stations { get; private set; }

        public MapRegion SelectedRegion {
            get { return _selectedRegion; }
            set {
                if (Equals(_selectedRegion, value)) return;
                _selectedRegion = value;
                NotifyOfPropertyChange(() => SelectedRegion);
            }
        }

        public StaStation SelectedStation {
            get { return _selectedStation; }
            set {
                if (Equals(value, _selectedStation)) return;
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
            }
        }

        public BindableCollection<InvType> SelectedItems {
            get { return _selectedItems; }
            private set {
                if (Equals(_selectedItems, value)) return;
                _selectedItems = value;
                NotifyOfPropertyChange(() => SelectedItems);
            }
        }


        public BindableCollection<MarketAnalyzerEntry> MarketAnalyzerResults {
            get { return _marketAnalyzerResults; }
            private set {
                if (Equals(_marketAnalyzerResults, value)) return;
                _marketAnalyzerResults = value;
                NotifyOfPropertyChange(() => MarketAnalyzerResults);
            }
        }

        public int DayLimit {
            get { return _dayLimit; }
            set {
                if (Equals(_dayLimit, value)) return;
                _dayLimit = value;
                NotifyOfPropertyChange(() => DayLimit);
            }
        }

        public void Handle(OrdersAddedEventArgs ordersAddedEventArgs) {
            ILookup<int, MarketAnalyzerEntry> lookup = MarketAnalyzerResults.ToLookup(f => f.InvType.TypeId);
            foreach (Order order in ordersAddedEventArgs.Orders) {
                if (lookup.Contains(order.TypeId)) {
                    lookup[order.TypeId].Single().Order = order;
                    MarketAnalyzerResults.NotifyOfPropertyChange();
                }
            }
        }

        protected override void OnInitialize() {
            TreeRootNodes = _dataService.BuildMarketTree(treeViewCheckBox_PropertyChanged);
            Regions = _dataService.Db.MapRegions.OrderBy(region => region.RegionName).ToList();
            SelectedRegion = Regions.Single(f => f.RegionId == 10000002);
            SelectedStation = SelectedRegion.StaStations.Single(station => station.StationId == 60003760);
        }

        private async void Analyze() {
            var busy = new BusyIndicator {IsBusy = true};
            var cts = new CancellationTokenSource();
            var progressVm = new AnalyzerProgressViewModel(cts);
            MarketAnalyzer res = await GetMarketAnalyzer(SelectedItems);
            LoadOrderData(res.Result);
            MarketAnalyzerResults = new BindableCollection<MarketAnalyzerEntry>(res.Result);
            busy.IsBusy = false;
        }

        private void LoadOrderData(IEnumerable<MarketAnalyzerEntry> items) {
            items.Apply(
                item =>
                    item.Order =
                        item.InvType.Orders.SingleOrDefault(
                            order => order.ApiKeyEntity_Id == ApplicationHelper.ActiveKeyEntity.Id));
        }

        public bool CanScannerLinkAction() {
            return MarketAnalyzerResults != null;
        }

        public void ScannerLinkAction() {
            //IEnumerable<long> list =
            //    MarketAnalyzerResults.Cast<MarketAnalyzerResult>()
            //        .Where(result => result.IsChecked)
            //        .Select(f => f.TypeId);
            //Uri uri = _eveMarketService.GetScannerLink(list.ToList());
            //var scannerVm = new ScannerLinkViewModel(uri);
            //_windowManager.ShowDialog(scannerVm);
        }


        private bool CanAddToOrders(ICollection<object> objects) {
            if (objects == null || !objects.Any())
                return false;
            List<MarketAnalyzerEntry> items = objects.Select(item => (MarketAnalyzerEntry) item).ToList();
            return items.All(item => item.Order == null);
        }

        private void AddToOrders(ICollection<object> objects) {
            if (objects == null || !objects.Any())
                return;
            List<MarketAnalyzerEntry> items = objects.Select(item => (MarketAnalyzerEntry) item).ToList();
            _eventAggregator.Publish(new AddToOrdersEventArgs(items));
        }

        public async void LoadOrders() {
            List<InvType> items =
                _dataService.Db.Orders.Where(order => order.ApiKeyEntity_Id == ApplicationHelper.ActiveKeyEntity.Id)
                    .Select(order => order.InvType).ToList();
            MarketAnalyzer res = await GetMarketAnalyzer(items);
            LoadOrderData(res.Result);
            MarketAnalyzerResults = new BindableCollection<MarketAnalyzerEntry>(res.Result);
        }

        private async Task<MarketAnalyzer> GetMarketAnalyzer(ICollection<InvType> items) {
            return await
                Task.Run(() => _eveMarketService.GetMarketAnalyzer(SelectedRegion, SelectedStation, items, DayLimit));
        }

        private void treeViewCheckBox_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var item = sender as InvType;
            if (e.PropertyName == "IsChecked") {
                if (item.IsChecked == true) {
                    SelectedItems.Add(item);
                }
                else if (item.IsChecked == false)
                    SelectedItems.Remove(item);
                else {
                    throw new NotImplementedException();
                }
            }
            TreeRootNodes.NotifyOfPropertyChange();
        }

    }
}