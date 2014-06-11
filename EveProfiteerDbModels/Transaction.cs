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
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public long TransactionId { get; set; }
        public int Quantity { get; set; }
        public int TypeId { get; set; }
        public decimal Price { get; set; }
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public long StationId { get; set; }
        public string StationName { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionFor { get; set; }
        public int ApiKeyEntity_Id { get; set; }
        public long JournalTransactionId { get; set; }
        public int ClientTypeId { get; set; }
        public decimal PerpetualAverageCost { get; set; }
        public int CurrentStock { get; set; }
        public int UnaccountedStock { get; set; }
        public decimal TaxLiability { get; set; }
        public decimal BrokerFee { get; set; }
    
        public virtual ApiKeyEntity ApiKeyEntity { get; set; }
        public virtual InvType InvType { get; set; }
        public virtual InvType ClientType { get; set; }
    }
}
