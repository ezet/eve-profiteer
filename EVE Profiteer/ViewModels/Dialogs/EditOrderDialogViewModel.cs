﻿using Caliburn.Micro;

namespace eZet.EveProfiteer.ViewModels.Dialogs {
    public class EditOrderDialogViewModel : Screen {
        private int _buyOrderTotal;
        private int _maxSellOrderTotal;
        private int _minSellOrderTotal;
        private bool _setBuyOrderTotal;
        private bool _setMaxSellOrderTotal;
        private bool _setMinSellOrderTotal;

        public bool SetBuyOrderTotal {
            get { return _setBuyOrderTotal; }
            set {
                if (value.Equals(_setBuyOrderTotal)) return;
                _setBuyOrderTotal = value;
                NotifyOfPropertyChange(() => SetBuyOrderTotal);
            }
        }

        public bool SetMinSellOrderTotal {
            get { return _setMinSellOrderTotal; }
            set {
                if (value.Equals(_setMinSellOrderTotal)) return;
                _setMinSellOrderTotal = value;
                NotifyOfPropertyChange(() => SetMinSellOrderTotal);
            }
        }

        public bool SetMaxSellOrderTotal {
            get { return _setMaxSellOrderTotal; }
            set {
                if (value.Equals(_setMaxSellOrderTotal)) return;
                _setMaxSellOrderTotal = value;
                NotifyOfPropertyChange(() => SetMaxSellOrderTotal);
            }
        }

        public int BuyOrderTotal {
            get { return _buyOrderTotal; }
            set {
                if (value == _buyOrderTotal) return;
                _buyOrderTotal = value;
                NotifyOfPropertyChange(() => BuyOrderTotal);
            }
        }

        public int MinSellOrderTotal {
            get { return _minSellOrderTotal; }
            set {
                if (value == _minSellOrderTotal) return;
                _minSellOrderTotal = value;
                NotifyOfPropertyChange(() => MinSellOrderTotal);
            }
        }

        public int MaxSellOrderTotal {
            get { return _maxSellOrderTotal; }
            set {
                if (value == _maxSellOrderTotal) return;
                _maxSellOrderTotal = value;
                NotifyOfPropertyChange(() => MaxSellOrderTotal);
            }
        }
    }
}