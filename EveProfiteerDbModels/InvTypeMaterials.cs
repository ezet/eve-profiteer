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
    
    public partial class InvTypeMaterials
    {
        public int TypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int Quantity { get; set; }
    
        public virtual InvType InvType { get; set; }
        public virtual InvType MaterialType { get; set; }
    }
}