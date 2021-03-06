﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Mvvm;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Services;
using eZet.EveProfiteer.Ui.Events;

namespace eZet.EveProfiteer.ViewModels.Modules {
    public sealed class TradeSummaryViewModel : ModuleViewModel {
        public enum ViewPeriodEnum {
            //Today,
            //Yesterday,
            Week,
            Month,
            All,
            Since,
            Period
        }

        private readonly TradeSummaryService _tradeSummaryService;
        private readonly IEventAggregator _eventAggregator;
        private ViewPeriodEnum _selectedViewPeriod;
        private TransactionAggregate _summary;

        public TradeSummaryViewModel(TradeSummaryService tradeSummaryService, IEventAggregator eventAggregator) {
            _tradeSummaryService = tradeSummaryService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            PeriodSelectorStart = DateTime.UtcNow.AddMonths(-1);
            PeriodSelectorEnd = DateTime.UtcNow;
            _selectedViewPeriod = ViewPeriodEnum.Week;
            ViewPeriodCommand = new DelegateCommand(ExecuteViewPeriod);
        }

        public ICommand ViewPeriodCommand { get; private set; }

        public DateTime ActualViewStart { get; private set; }

        public DateTime ActualViewEnd { get; private set; }

        public DateTime PeriodSelectorStart { get; set; }

        public DateTime PeriodSelectorEnd { get; set; }

        public bool NeedRefresh { get; set; }

        public IEnumerable<ViewPeriodEnum> ViewPeriodValues {
            get { return Enum.GetValues(typeof(ViewPeriodEnum)).Cast<ViewPeriodEnum>(); }
        }

        public ViewPeriodEnum SelectedViewPeriod {
            get { return _selectedViewPeriod; }
            set {
                _selectedViewPeriod = value;
                NotifyOfPropertyChange(() => SelectedViewPeriod);
            }
        }

        public TransactionAggregate Summary {
            get { return _summary; }
            private set {
                if (Equals(value, _summary)) return;
                _summary = value;
                NotifyOfPropertyChange();
            }
        }

        public void ExecuteViewPeriod() {
            ViewPeriod().ConfigureAwait(false);
        }

        public Task ViewPeriod() {
            switch (SelectedViewPeriod) {
                case ViewPeriodEnum.All:
                    ActualViewStart = DateTime.MinValue;
                    ActualViewEnd = DateTime.MaxValue;
                    break;
                //case ViewPeriodEnum.Today:
                //    ActualViewStart = DateTime.UtcNow.Date;
                //    ActualViewEnd = DateTime.MaxValue;
                //    break;
                //case ViewPeriodEnum.Yesterday:
                //    ActualViewStart = DateTime.UtcNow.AddDays(-1).Date;
                //    ActualViewEnd = DateTime.UtcNow.Date;
                //    break;
                case ViewPeriodEnum.Week:
                    ActualViewStart = DateTime.UtcNow.AddDays(-7);
                    ActualViewEnd = DateTime.MaxValue;
                    break;
                case ViewPeriodEnum.Month:
                    ActualViewStart = DateTime.UtcNow.AddMonths(-1);
                    ActualViewEnd = DateTime.MaxValue;
                    break;
                case ViewPeriodEnum.Since:
                    ActualViewStart = PeriodSelectorStart;
                    ActualViewEnd = DateTime.MaxValue;
                    break;
                case ViewPeriodEnum.Period:
                    ActualViewStart = PeriodSelectorStart;
                    ActualViewEnd = PeriodSelectorEnd;
                    break;
            }
            return load(ActualViewStart, ActualViewEnd);
        }


        private async Task load(DateTime start, DateTime end) {
            _eventAggregator.PublishOnUIThread(new StatusEvent(this, "Loading..."));
            List<Transaction> transactions = await
                _tradeSummaryService.GetTransactions(start, end).ConfigureAwait(false);
            _eventAggregator.PublishOnUIThread(new StatusEvent(this, "Analyzing..."));
            Summary = new TransactionAggregate(transactions.GroupBy(t => t.TransactionDate.Date));
            _eventAggregator.PublishOnUIThread(new StatusEvent(this, "Analysis complete"));
        }

    }
}