﻿using System.Windows;
using DevExpress.Xpf.Mvvm.UI;
using eZet.EveOnlineDbModels;

namespace eZet.EveProfiteer.Ui.Converters {
    public class RoutedPropertyChangedEventArgsConverter : EventArgsConverterBase<RoutedPropertyChangedEventArgs<object>> {
        protected override object Convert(RoutedPropertyChangedEventArgs<object> args) {
            return args.NewValue as InvType;
        }
    }
}
