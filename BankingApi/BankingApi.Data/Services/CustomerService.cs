using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public class CustomerService
    {
        private readonly BankingContext _ctx;
        private readonly IMapper _mapper;

        public CustomerService(BankingContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all customer sorted by last name, first name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerDto> GetAll()
            => _mapper.ProjectTo<CustomerDto>(_ctx.Customers.OrderBy(s => s.LastName).ThenBy(s => s.FirstName));

        /// <summary>
        /// Returns a customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerDto> GetByIdAsync(Guid id)
        {
            var customerEntity = await _ctx.Customers.FindAsync(id);

            if (customerEntity is null)
            {
                return null;
            }

            return _mapper.Map<CustomerDto>(customerEntity);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        public async Task<CustomerDto> CreateAsync(NewCustomerDto newCustomer)
        {
            var customerEntity = _mapper.Map<Customer>(newCustomer);

            customerEntity.Id = Guid.NewGuid();
            customerEntity.CreatedAt = DateTime.UtcNow;

            _ctx.Customers.Add(customerEntity);

            if (await _ctx.SaveChangesAsync() == 1)
            {
                return _mapper.Map<CustomerDto>(customerEntity);
            }

            return null;
        }

        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(CustomerDto customer)
        {
            var customerEntity = await _ctx.Customers.FindAsync(customer.Id);

            if (customerEntity is null)
            {
                return 0;
            }

            _mapper.Map(customer, customerEntity);
            customerEntity.UpdatedAt = DateTime.UtcNow;

            return await _ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var customerEntity = await _ctx.Customers.FindAsync(id);

            if (customerEntity is null)
            {
                return false;
            }

            _ctx.Customers.Remove(customerEntity);

            return await _ctx.SaveChangesAsync() == 1;
        }
    }
}
