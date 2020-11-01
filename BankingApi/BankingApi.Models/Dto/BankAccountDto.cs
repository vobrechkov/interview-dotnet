using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Dto
{
    public class BankAccountDto : NewBankAccountDto
    {
        public Guid Id { get; set; }
        [DataType(DataType.Currency)]
        public decimal PostedBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
