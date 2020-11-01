using System;

namespace BankingApi.Models.Dto
{
    public class TransactionSummaryDto
    {
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid TransactionId { get; set; }        
        public Guid CustomerId { get; set; }        
        public Guid BankAccountId { get; set; }        
        public decimal CurrentBalance { get; set; }        
    }
}
