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
    
    public partial class ItemCost
    {
        public int Id { get; set; }
        public int InvTypes_TypeId { get; set; }
        public decimal MovingAverage { get; set; }
        public Nullable<decimal> Fifo { get; set; }
        public Nullable<decimal> Lifo { get; set; }
        public int ApiKeyEntities_Id { get; set; }
    
        public virtual ApiKeyEntity ApiKeyEntity { get; set; }
        public virtual InvType invType { get; set; }
    }
}
