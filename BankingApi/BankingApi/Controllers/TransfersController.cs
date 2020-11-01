using BankingApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly BankAccountService _bankAccountService;

        public TransfersController(BankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost("from/{sourceAccountId:guid}/to/{targetAccountId:guid}/amount/{amount:decimal}")]
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
