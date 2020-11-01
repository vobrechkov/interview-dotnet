using BankingApi.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
