using BankingApi.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApi.Models.Entities
{
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Number { get; set; }
        public string RoutingNumber { get; set; }
        public Guid CustomerId { get; set; }
        public AccountType Type { get; set; }
        public string DisplayName { get; set; }
        public decimal PostedBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
