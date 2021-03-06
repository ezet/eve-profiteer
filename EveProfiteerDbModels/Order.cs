//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eZet.EveProfiteer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int BuyQuantity { get; set; }
        public decimal MaxBuyPrice { get; set; }
        public int MinSellQuantity { get; set; }
        public decimal MinSellPrice { get; set; }
        public int MaxSellQuantity { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public double AvgVolume { get; set; }
        public decimal CurrentBuyPrice { get; set; }
        public decimal CurrentSellPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public int ApiKeyEntity_Id { get; set; }
        public bool IsSellOrder { get; set; }
        public bool IsBuyOrder { get; set; }
        public string Notes { get; set; }
        public bool AutoProcess { get; set; }
        public int StationId { get; set; }
    
        public virtual ApiKeyEntity ApiKeyEntity { get; set; }
        public virtual InvType InvType { get; set; }
        public virtual StaStation staStation { get; set; }
    }
}
