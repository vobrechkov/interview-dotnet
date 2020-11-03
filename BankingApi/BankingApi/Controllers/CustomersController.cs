using BankingApi.Data.Services;
using BankingApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IBankAccountService _accountService;

        public CustomersController(ICustomerService customerService, IBankAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }

        // GET: api/customers
        [HttpGet]
        public IActionResult Get() => Ok(_customerService.GetAll());

        // GET api/customers/{guid}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET api/customers/{guid}/bankaccounts
        [HttpGet("{customerId:guid}/bankaccounts")]
        public IActionResult GetByCustomerIdAsync(Guid customerId)
            => Ok(_accountService.GetAllByCustomerId(customerId));

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCustomerDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _customerService.CreateAsync(customer);

            if (result is null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        // PUT api/customers/{guid}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CustomerDto customer)
        {
            if (!ModelState.IsValid || id != customer.Id)
            {
                return BadRequest();
            }

            if (await _customerService.UpdateAsync(customer) != 1)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/customers/{guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _customerService.DeleteAsync(id))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
