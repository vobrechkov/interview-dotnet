using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Entities
{
    public class Transfer
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SourceTransactionId { get; set; }
        public Guid DestinationTransactionId { get; set; }        
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Transaction SourceTransaction { get; set; }
        public virtual Transaction DestinationTransaction { get; set; }
    }
}
