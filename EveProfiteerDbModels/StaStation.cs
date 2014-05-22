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
    
    public partial class StaStation
    {
        public int StationId { get; set; }
        public Nullable<short> Security { get; set; }
        public Nullable<double> DockingCostPerVolume { get; set; }
        public Nullable<double> MaxShipVolumeDockable { get; set; }
        public Nullable<int> OfficeRentalCost { get; set; }
        public Nullable<byte> OperationId { get; set; }
        public Nullable<int> StationTypeId { get; set; }
        public Nullable<int> CorporationId { get; set; }
        public Nullable<int> SolarSystemId { get; set; }
        public Nullable<int> ConstellationId { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string StationName { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
        public Nullable<double> Z { get; set; }
        public Nullable<double> ReprocessingEfficiency { get; set; }
        public Nullable<double> ReprocessingStationsTake { get; set; }
        public Nullable<byte> ReprocessingHangarFlag { get; set; }
    
        public virtual MapRegion MapRegion { get; set; }
    }
}
