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
    
    public partial class MapRegion
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
        public Nullable<double> Z { get; set; }
        public Nullable<double> XMin { get; set; }
        public Nullable<double> XMax { get; set; }
        public Nullable<double> YMin { get; set; }
        public Nullable<double> YMax { get; set; }
        public Nullable<double> ZMin { get; set; }
        public Nullable<double> ZMax { get; set; }
        public Nullable<int> FactionId { get; set; }
        public Nullable<double> Radius { get; set; }
    }
}