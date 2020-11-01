using System;

namespace BankingApi.Models.Dto
{
    public class TransferDto
    {
        public Guid Id { get; set; }
        public Guid SourceTransactionId { get; set; }
        public Guid DestinationTransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
