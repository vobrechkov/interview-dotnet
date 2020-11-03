using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Entities
{
    public class BankAccountNumber
    {
        public BankAccountNumber()
        {
            this.Id = Guid.NewGuid();
            this.GeneratedAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }
        public int LastNumber { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
