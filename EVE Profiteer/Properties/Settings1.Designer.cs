﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eZet.EveProfiteer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("dd/MM/yyyy")]
        public string DateFormat {
            get {
                return ((string)(this["DateFormat"]));
            }
            set {
                this["DateFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Color BuyOrderColor {
            get {
                return ((global::System.Drawing.Color)(this["BuyOrderColor"]));
            }
            set {
                this["BuyOrderColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Color SellOrderColor {
            get {
                return ((global::System.Drawing.Color)(this["SellOrderColor"]));
            }
            set {
                this["SellOrderColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("224, 224, 224")]
        public global::System.Drawing.Color ActiveOrderColor {
            get {
                return ((global::System.Drawing.Color)(this["ActiveOrderColor"]));
            }
            set {
                this["ActiveOrderColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Color InactiveOrderColor {
            get {
                return ((global::System.Drawing.Color)(this["InactiveOrderColor"]));
            }
            set {
                this["InactiveOrderColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ActiveCaption")]
        public global::System.Drawing.Color DonchianChannelColor {
            get {
                return ((global::System.Drawing.Color)(this["DonchianChannelColor"]));
            }
            set {
                this["DonchianChannelColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Blue")]
        public global::System.Drawing.Color DonchianCenterColor {
            get {
                return ((global::System.Drawing.Color)(this["DonchianCenterColor"]));
            }
            set {
                this["DonchianCenterColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("InfoText")]
        public global::System.Drawing.Color AveragePriceColor {
            get {
                return ((global::System.Drawing.Color)(this["AveragePriceColor"]));
            }
            set {
                this["AveragePriceColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Green")]
        public global::System.Drawing.Color Moving20DayColor {
            get {
                return ((global::System.Drawing.Color)(this["Moving20DayColor"]));
            }
            set {
                this["Moving20DayColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color Moving5DayColor {
            get {
                return ((global::System.Drawing.Color)(this["Moving5DayColor"]));
            }
            set {
                this["Moving5DayColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HotTrack")]
        public global::System.Drawing.Color VolumeColor {
            get {
                return ((global::System.Drawing.Color)(this["VolumeColor"]));
            }
            set {
                this["VolumeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000002")]
        public int DefaultRegionId {
            get {
                return ((int)(this["DefaultRegionId"]));
            }
            set {
                this["DefaultRegionId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60003760")]
        public int DefaultStationId {
            get {
                return ((int)(this["DefaultStationId"]));
            }
            set {
                this["DefaultStationId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-2")]
        public double SellPriceOffset {
            get {
                return ((double)(this["SellPriceOffset"]));
            }
            set {
                this["SellPriceOffset"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-2")]
        public double BuyPriceOffset {
            get {
                return ((double)(this["BuyPriceOffset"]));
            }
            set {
                this["BuyPriceOffset"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public double MinProfitMargin {
            get {
                return ((double)(this["MinProfitMargin"]));
            }
            set {
                this["MinProfitMargin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public double MaxProfitMargin {
            get {
                return ((double)(this["MaxProfitMargin"]));
            }
            set {
                this["MaxProfitMargin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200000000")]
        public decimal MaxBuyOrderTotal {
            get {
                return ((decimal)(this["MaxBuyOrderTotal"]));
            }
            set {
                this["MaxBuyOrderTotal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20000000")]
        public decimal MinSellOrderTotal {
            get {
                return ((decimal)(this["MinSellOrderTotal"]));
            }
            set {
                this["MinSellOrderTotal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500000000")]
        public decimal MaxSellOrderTotal {
            get {
                return ((decimal)(this["MaxSellOrderTotal"]));
            }
            set {
                this["MaxSellOrderTotal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.75")]
        public double TaxRate {
            get {
                return ((double)(this["TaxRate"]));
            }
            set {
                this["TaxRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.73")]
        public double BrokerFeeRate {
            get {
                return ((double)(this["BrokerFeeRate"]));
            }
            set {
                this["BrokerFeeRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("dd/MM/yy hh:mm:ss")]
        public string DateTimeFormat {
            get {
                return ((string)(this["DateTimeFormat"]));
            }
            set {
                this["DateTimeFormat"] = value;
            }
        }
    }
}
