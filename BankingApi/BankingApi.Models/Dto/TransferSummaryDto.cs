using System;

namespace BankingApi.Models.Dto
{
    public class TransferSummaryDto
    {
        public decimal Amount { get; set; }
        public Guid TransferId { get; set; }        
        public DateTime TransferDate { get; set; }
        public Guid SourceBankAccountId { get; set; }
        public Guid DestinationBankAccountId { get; set; }
    }
}
