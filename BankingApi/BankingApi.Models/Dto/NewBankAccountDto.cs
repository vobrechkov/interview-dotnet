using BankingApi.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Dto
{
    public class NewBankAccountDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public AccountType Type { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
