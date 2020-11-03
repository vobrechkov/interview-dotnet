using System;

namespace BankingApi.Models.Dto
{
    public class TransferSummaryDto
    {
        public decimal Amount { get; set; }
        public Guid TransferId { get; set; }        
        public DateTime TransferDate { get; set; }
        public string SourceAccountNumber { get; set; }
        public decimal SourceAccountBalance { get; set; }
        public string DestinationAccountNumber { get; set; }
        public decimal DestinationAccountBalance { get; set; }
    }
}
