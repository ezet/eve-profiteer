﻿using System;
using eZet.EveProfiteer.Models;
using eZet.EveProfiteer.Ui.Events;
using eZet.EveProfiteer.ViewModels.Modules;

namespace eZet.EveProfiteer.Events {
    public class ViewMarketOrderEvent : ModuleEvent {
        public InvType InvType { get; private set; }

        public ViewMarketOrderEvent(InvType invType) {
            InvType = invType;
        }

        public override Type GetTabType() {
            return typeof (MarketOrdersViewModel);
        }
    }
}
