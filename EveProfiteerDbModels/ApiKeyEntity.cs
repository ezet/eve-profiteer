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
    
    public partial class ApiKeyEntity
    {
        public ApiKeyEntity()
        {
            this.JournalEntries = new HashSet<JournalEntry>();
            this.Transactions = new HashSet<Transaction>();
            this.ApiKeys = new HashSet<ApiKey>();
        }
    
        public int Id { get; set; }
        public long EntityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<JournalEntry> JournalEntries { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<ApiKey> ApiKeys { get; set; }
    }
}
