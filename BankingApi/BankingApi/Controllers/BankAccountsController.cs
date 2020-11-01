using BankingApi.Data.Services;
using BankingApi.Models.Dto;
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
        private readonly BankAccountService _bankAccountService;        

        public BankAccountsController(BankAccountService bankAccountService, IOptions<ApiBehaviorOptions> behaviorOptions)
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

            return Ok(bankAccounts.Select(s => new { s.Id, s.DisplayName }));
        }

        // GET api/bankaccounts/{guid}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var bankAccount = await _bankAccountService.GetByIdAsync(id);

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

            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        // PUT api/bankaccounts/{guid}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] BankAccountDto bankAccount)
        {
            if (!ModelState.IsValid || id != bankAccount.Id)
            {
                return BadRequest();
            }

            if (await _bankAccountService.UpdateAsync(bankAccount) != 1)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/bankaccounts/{guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!await _bankAccountService.DeleteAsync(id))
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("{bankAccountId:guid}/deposit/{amount:decimal}")]
        public async Task<IActionResult> AddDepositAsync(Guid bankAccountId, decimal amount)
        {
            var bankAccount = await _bankAccountService.GetByIdAsync(bankAccountId);

            if (bankAccount == null)
            {
                ModelState.AddModelError(nameof(bankAccountId), "Bank account not found");                
            }

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be greater than zero");
            }

            if (!ModelState.IsValid)
            {
                return ModelStateValidationBadRequest();
            }

            var result = await _bankAccountService.CreateTransactionAsync(bankAccountId, Models.Enumerations.TransactionType.Deposit, amount);

            if (result.Errors.Any())
            {
                result.Errors.ForEach(e => ModelState.AddModelError("", e));
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpPost("{sourceAccountId:guid}/transfer/{targetAccountId:guid}/amount/{amount:decimal}")]
        public async Task<IActionResult> Post(Guid sourceAccountId, Guid targetAccountId, decimal amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bankAccountService.TranserAsync(sourceAccountId, targetAccountId, amount);

            if (result.Errors.Any())
            {
                result.Errors.ForEach(e => ModelState.AddModelError("", e));
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
