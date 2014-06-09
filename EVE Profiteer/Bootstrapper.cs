using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using eZet.EveProfiteer.Framework;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Repository;
using eZet.EveProfiteer.Services;
using eZet.EveProfiteer.ViewModels;
using eZet.EveProfiteer.ViewModels.Dialogs;
using eZet.EveProfiteer.ViewModels.Tabs;

namespace eZet.EveProfiteer {
    public class Bootstrapper : BootstrapperBase {

        private TraceSource _trace = new TraceSource("Main");
        private SimpleContainer _container;

        public Bootstrapper() {
            Initialize();
        }

        protected override void Configure() {

            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // DbContexts
            _container.PerRequest<EveProfiteerDbEntities>();
            _container.Singleton<EveProfiteerDataService>();
            //_container.PerRequest<EveProfiteerDbEntities>();

            // Repositories
            _container.PerRequest<IRepository<Transaction>, DbContextRepository<Transaction, EveProfiteerDbEntities>>();
            _container.PerRequest<IRepository<JournalEntry>, DbContextRepository<JournalEntry, EveProfiteerDbEntities>>();
            _container.PerRequest<IRepository<ApiKey>, DbContextRepository<ApiKey, EveProfiteerDbEntities>>();
            _container.PerRequest<IRepository<ApiKeyEntity>, DbContextRepository<ApiKeyEntity, EveProfiteerDbEntities>>();
            _container.PerRequest<IRepository<Order>, DbContextRepository<Order, EveProfiteerDbEntities>>();


            // Services
            _container.PerRequest<EveApiService>();
            _container.PerRequest<TransactionService>();
            _container.PerRequest<EveMarketService>();
            _container.PerRequest<OrderXmlService>();
            _container.PerRequest<RepositoryService<ApiKey>>();
            _container.PerRequest<RepositoryService<ApiKeyEntity>>();
            _container.PerRequest<KeyManagementService>();
            _container.PerRequest<AssetService>();


            // View Models
            _container.PerRequest<IShell, ShellViewModel>();
            _container.PerRequest<ManageKeysViewModel>();
            _container.PerRequest<AddKeyViewModel>();
            _container.PerRequest<EditKeyViewModel>();
            _container.PerRequest<TradeSummaryViewModel>();
            _container.PerRequest<MarketAnalyzerViewModel>();
            _container.PerRequest<OrderEditorViewModel>();
            _container.PerRequest<TradeAnalyzerViewModel>();
            _container.PerRequest<TradeDetailsViewModel>();
            _container.PerRequest<MarketBrowserViewModel>();
            _container.PerRequest<AssetsViewModel>();
            _container.PerRequest<ProductionViewModel>();
        }

        protected override object GetInstance(Type service, string key) {
            object instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}