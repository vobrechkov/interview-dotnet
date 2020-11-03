using BankingApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public interface ICustomerService
    {
        public IEnumerable<CustomerDto> GetAll();
        public Task<CustomerDto> GetByIdAsync(Guid id);
        public Task<CustomerDto> CreateAsync(NewCustomerDto newCustomer);
        public Task<int> UpdateAsync(CustomerDto customer);
        public Task<bool> DeleteAsync(Guid id);
    }
}
