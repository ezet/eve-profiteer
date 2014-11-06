using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DevExpress.Mvvm;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using eZet.EveProfiteer.Events;
using eZet.EveProfiteer.Framework;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Properties;
using eZet.EveProfiteer.Services;
using eZet.EveProfiteer.Ui;
using eZet.EveProfiteer.Ui.Events;
using eZet.EveProfiteer.Util;
using eZet.EveProfiteer.ViewModels.Dialogs;
using eZet.EveProfiteer.Views;

namespace eZet.EveProfiteer.ViewModels {
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell,
        IHandle<StatusChangedEventArgs>,
        IHandle<IActivateTabEvent> {
        private readonly IEventAggregator _eventAggregator;
        private readonly ModuleService _moduleService;
        private readonly ShellService _shellService;
        private readonly TraceSource _trace = new TraceSource("EveProfiteer", SourceLevels.All);
        private readonly IWindowManager _windowManager;
        private string _statusMessage;

        public ShellViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ShellService shellService,
            ModuleService moduleService) {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _shellService = shellService;
            _moduleService = moduleService;
            DisplayName = "EVE Profiteer";
            StatusLocked = false;

            StatusMessage = "Initializing...";
            _eventAggregator.Subscribe(this);

            OpenKeyManagerCommand = new DelegateCommand(ExecuteOpenKeyManager);
            OpenSettingsCommand = new DelegateCommand(ExecuteOpenSettings);
            UpdateTransactionsCommand = new DelegateCommand(ExecuteUpdateTransactions);
            ProcessAllTransactionsCommand = new DelegateCommand(ExecuteProcessAllTransactions);
            UpdateAssetsCommand = new DelegateCommand(ExecuteUpdateAssets);
            UpdateJournalCommand = new DelegateCommand(ExecuteUpdateJournal);
            UpdateApiCommand = new DelegateCommand(ExecuteUpdateApi);
            UpdateRefTypesCommand = new DelegateCommand(ExecuteUpdateRefTypes);
            ProcessUnaccountedTransactionsCommand = new DelegateCommand(ExecuteProcessUnaccountedTransactions);
            UpdateIndustryJobsCommand = new DelegateCommand(ExecuteUpdateIndystryJobs);
            UpdateMarketOrdersCommand = new DelegateCommand(ExecuteUpdateMarketOrders);
            ActivateTabCommand = new DelegateCommand<Type>(ExecuteActivateTab);

            Initialize = InitializeAsync();
        }

        protected override async void OnInitialize() {
            await Initialize;
        }

        public Task Initialize { get; private set; }

        public BindableCollection<ApiKeyEntity> Entities { get; set; }

        public ICommand OpenSettingsCommand { get; set; }

        public ModuleViewModel ActiveModel { get; private set; }

        public string StatusMessage {
            get { return _statusMessage; }
            set {
                if (value == _statusMessage) return;
                _statusMessage = value;
                NotifyOfPropertyChange(() => StatusMessage);
            }
        }

        public bool StatusLocked { get; set; }

        #region COMMANDS

        public ICommand ActivateTabCommand { get; private set; }

        public ICommand UpdateMarketOrdersCommand { get; private set; }

        public ICommand UpdateIndustryJobsCommand { get; private set; }

        public ICommand ProcessUnaccountedTransactionsCommand { get; private set; }

        public ICommand ProcessAllTransactionsCommand { get; private set; }

        public ICommand UpdateAssetsCommand { get; private set; }

        public ICommand OpenKeyManagerCommand { get; private set; }

        public ICommand UpdateTransactionsCommand { get; private set; }

        public ICommand UpdateJournalCommand { get; private set; }

        public ICommand UpdateApiCommand { get; private set; }

        public ICommand UpdateRefTypesCommand { get; private set; }

        #endregion

        public void Handle(IActivateTabEvent message) {
            ExecuteActivateTab(message.GetTabType());
        }

        public void Handle(StatusChangedEventArgs message) {
            _trace.TraceEvent(TraceEventType.Information, 0, "StatusChangedEventArgs: {0}", message.Status);
            if (!StatusLocked) {
                StatusMessage = message.Status;
            }
        }

        private async Task InitializeAsync() {
            IEnumerable<ApiKeyEntity> entities = await _shellService.GetAllActiveEntities().ConfigureAwait(false);
            Entities = new BindableCollection<ApiKeyEntity>(entities);
            int activeEntityId = Settings.Default.ActiveEntity;
            if (activeEntityId != 0) {
                ApiKeyEntity entity = Entities.Single(e => e.Id == activeEntityId);
                if (entity != null) ApplicationHelper.ActiveEntity = entity;
            }
        }

        private void ExecuteOpenSettings() {
            bool? result = _windowManager.ShowDialog(IoC.Get<SettingsViewModel>());
            if (result.GetValueOrDefault()) {
                Settings.Default.Save();
                _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Settings Saved"));
            }
            else {
                Settings.Default.Reload();
                _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Settings Discarded"));
            }
        }

        private void ExecuteActivateTab(Type type) {
            ModuleViewModel vm = _moduleService.GetModule(type);
            activateTab(vm);
            ActiveModel = vm;
        }

        private void activateTab(ModuleViewModel viewModel) {
            ActivateItem(viewModel);
            var view = (ShellView) GetView();
            BaseLayoutItem tab = view.ModuleHost.VisiblePages.SingleOrDefault(page => page.DataContext == viewModel);
            if (tab == null) {
                tab = view.DockLayoutManager.ClosedPanels.Single(page => page.DataContext == viewModel);
                tab.Closed = false;
            }
            view.DockLayoutManager.DockController.Activate(tab);
        }

        private void initDefaultModules() {
            activateTab(_moduleService.GetDefault());
        }

        protected override void OnViewLoaded(object view) {
            var shellView = (ShellView) view;
            initDefaultModules();
            initializeRibbon(shellView);
            base.OnViewLoaded(view);
        }

        private void initializeRibbon(ShellView view) {
            foreach (ModuleConfiguration config in _moduleService.Configurations) {
                var button = new BarButtonItem();
                button.Command = ActivateTabCommand;
                button.Content = config.DisplayName;
                button.CommandParameter = config.Type;
                button.LargeGlyph = GetImageStream(config.LargeGlyph);
                button.Glyph = GetImageStream(config.Glyph);
                switch (config.Category) {
                    case ModuleConfiguration.ModuleCategory.Trade:
                        view.TradeModules.ItemLinks.Add(button);
                        break;
                    case ModuleConfiguration.ModuleCategory.Industry:
                        view.IndustryModules.ItemLinks.Add(button);
                        break;
                    case ModuleConfiguration.ModuleCategory.Basic:
                        view.MiscModules.ItemLinks.Add(button);
                        break;
                }
            }
        }

        public static BitmapSource GetImageStream(Image myImage) {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
                Imaging.CreateBitmapSourceFromHBitmap(
                    bmpPt,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);
            return bitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        private void ExecuteOpenKeyManager() {
            _windowManager.ShowDialog(IoC.Get<KeyManagerViewModel>());
        }

        private async void ExecuteUpdateApi() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating API..."));
            Task transactions = Task.Run(() => updateTransactions());
            Task journal = Task.Run(() => updateJournal());
            Task<int> marketOrders = Task.Run(() => updateMarketOrders());
            Task assets = Task.Run(() => updateAssets());
            await Task.WhenAll(assets, transactions, journal, marketOrders).ConfigureAwait(false);
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("API Update Complete"));
        }

        private async void ExecuteUpdateAssets() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Assets..."));
            await updateAssets();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Assets Updated"));
        }


        private async void ExecuteUpdateTransactions() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Transactions..."));
            await updateTransactions();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Transactions Updated"));
        }


        private async void ExecuteUpdateJournal() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Journal..."));
            await updateJournal();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Journal Updated"));
        }

        private async void ExecuteUpdateMarketOrders() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Market Orders..."));
            int result = await updateMarketOrders();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Market Orders Updated ({0})", result));
        }

        private async void ExecuteUpdateRefTypes() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Reference Types..."));
            await _shellService.UpdateRefIdsAsync().ConfigureAwait(false);
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Reference Types Updated"));
        }

        private async void ExecuteProcessAllTransactions() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Processing All Transactions..."));
            await _shellService.ProcessAllTransactionsAsync();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("All Transactions Processed"));
        }


        private async void ExecuteUpdateIndystryJobs() {
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Updating Industry Jobs..."));
            await _shellService.UpdateIndustryJobs();
            _eventAggregator.PublishOnUIThread(new StatusChangedEventArgs("Industry Jobs Updated"));
            _eventAggregator.PublishOnBackgroundThread(new IndustryJobsUpdatedEvent());
        }

        private async void ExecuteProcessUnaccountedTransactions() {
            await _shellService.ProcessUnaccountedTransactionsAsync().ConfigureAwait(false);
        }

        private async Task updateAssets() {
            int result = await _shellService.UpdateAssetsAsync().ConfigureAwait(false);
            if (result > 0) {
                _eventAggregator.PublishOnBackgroundThread(new AssetsUpdatedEvent());
            }
        }

        private async Task updateTransactions() {
            int result = await _shellService.UpdateTransactionsAsync().ConfigureAwait(false);
            if (result > 0) {
                _eventAggregator.PublishOnBackgroundThread(new TransactionsUpdatedEvent());
            }
        }

        private async Task updateJournal() {
            int result = await _shellService.UpdateJournalAsync().ConfigureAwait(false);
            if (result > 0) {
                _eventAggregator.PublishOnBackgroundThread(new JournalUpdatedEvent());
            }
        }

        private async Task<int> updateMarketOrders() {
            // TODO fix this
            return 0;
            int result = await _shellService.UpdateMarketOrdersAsync().ConfigureAwait(false);
            if (result > 0) {
                _eventAggregator.PublishOnBackgroundThread(new MarketOrdersUpdatedEvent());
            }
            return result;
        }
    }
}