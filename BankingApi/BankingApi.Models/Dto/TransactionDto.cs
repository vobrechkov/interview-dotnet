using BankingApi.Models.Enumerations;
using System;

namespace BankingApi.Models.Dto
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
