﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EveProfiteerDbEntities : DbContext
    {
        public EveProfiteerDbEntities()
            : base("name=EveProfiteerDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApiKeyEntity> ApiKeyEntities { get; set; }
        public virtual DbSet<ApiKey> ApiKeys { get; set; }
        public virtual DbSet<JournalEntry> JournalEntries { get; set; }
        public virtual DbSet<InvMarketGroup> InvMarketGroups { get; set; }
        public virtual DbSet<InvType> InvTypes { get; set; }
        public virtual DbSet<MapRegion> MapRegions { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }
}
