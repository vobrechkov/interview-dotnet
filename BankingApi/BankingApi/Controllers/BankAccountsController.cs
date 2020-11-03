using BankingApi.Data.Services;
using BankingApi.Models.Dto;
using BankingApi.Models.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : BaseController
    {
        private readonly IBankAccountService _bankAccountService;        

        public BankAccountsController(IBankAccountService bankAccountService, IOptions<ApiBehaviorOptions> behaviorOptions)
            : base(behaviorOptions)
        {
            _bankAccountService = bankAccountService;
        }

        // GET: api/bankaccounts
        [HttpGet]
        public IActionResult Get() => Ok(_bankAccountService.GetAll());

        [HttpGet("list")]
        [HttpGet("list/customer/{customerId:guid}")]
        public IActionResult GetList(Guid? customerId)
        {
            var bankAccounts = customerId.HasValue ?
                _bankAccountService.GetAllByCustomerId((Guid)customerId) :
                _bankAccountService.GetAll();

            return Ok(bankAccounts.Select(s => new { s.Number, s.DisplayName }));
        }

        // GET api/bankaccounts/0000000000
        [HttpGet("{number}")]
        public async Task<IActionResult> GetByIdAsync(string number)
        {
            var bankAccount = await _bankAccountService.GetByIdAsync(number);

            if (bankAccount is null)
            {
                return NotFound();
            }

            return Ok(bankAccount);
        }        

        // POST api/bankaccounts
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]NewBankAccountDto newBankAccount)
        {
            if (await _bankAccountService.ExistsAsync(newBankAccount.CustomerId, newBankAccount.DisplayName))
            {
                ModelState.AddModelError(nameof(newBankAccount.DisplayName), "Bank account display name is taken");
            }

            if (!ModelState.IsValid)
            {
                return ModelStateValidationBadRequest();
            }

            var result = await _bankAccountService.CreateAsync(newBankAccount);

            if (result is null)
            {
                return BadRequest(result);
            }

            return CreatedAtAction("Get", new { id = result.Number }, result);
        }

        // PUT api/bankaccounts/{guid}
        [HttpPut("{number}")]
        public async Task<IActionResult> UpdateAsync(string number, [FromBody] BankAccountDto bankAccount)
        {
            if (number != bankAccount.Number)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            if (await _bankAccountService.UpdateAsync(bankAccount) != 1)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/bankaccounts/0000000000
        [HttpDelete("{number}")]
        public async Task<IActionResult> DeleteAsync(string number)
        {
            if (!await _bankAccountService.DeleteAsync(number))
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST api/bankaccounts/0000000000/deposit/50.00
        [HttpPost("{bankAccountNumber}/deposit/{amount:decimal}")]
        public async Task<IActionResult> AddDepositAsync(string bankAccountNumber, decimal amount)
        {
            var bankAccount = await _bankAccountService.GetByIdAsync(bankAccountNumber);

            if (bankAccount == null)
            {
                ModelState.AddModelError(nameof(bankAccountNumber), "Bank account not found");                
            }

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be greater than zero");
            }

            if (!ModelState.IsValid)
            {
                return ModelStateValidationBadRequest();
            }

            var result = await _bankAccountService.CreateTransactionAsync(bankAccountNumber, TransactionType.Deposit, amount);

            if (result.Errors.Any())
            {
                result.Errors.ForEach(e => ModelState.AddModelError("", e));
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpPost("{sourceAccountNumber}/transfer/{targetAccountNumber}/amount/{amount:decimal}")]
        public async Task<IActionResult> Post(string sourceAccountNumber, string targetAccountNumber, decimal amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bankAccountService.TranserAsync(sourceAccountNumber, targetAccountNumber, amount);

            if (result.Errors.Any())
            {
                result.Errors.ForEach(e => ModelState.AddModelError("", e));
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
