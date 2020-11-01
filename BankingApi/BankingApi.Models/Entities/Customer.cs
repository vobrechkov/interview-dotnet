using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApi.Models.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InstitutionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual IList<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}
