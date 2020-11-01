using BankingApi.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Entities
{
    public class BankAccount
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public AccountType Type { get; set; }
        public string DisplayName { get; set; }
        public decimal PostedBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
