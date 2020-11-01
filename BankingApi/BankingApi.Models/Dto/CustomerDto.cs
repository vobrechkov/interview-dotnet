using System;
using System.Collections.Generic;

namespace BankingApi.Models.Dto
{
    public class CustomerDto : NewCustomerDto
    {
        public CustomerDto()
        {
            BankAccounts = new List<BankAccountDto>();
        }

        public Guid Id { get; set; }

        public virtual IList<BankAccountDto> BankAccounts { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
