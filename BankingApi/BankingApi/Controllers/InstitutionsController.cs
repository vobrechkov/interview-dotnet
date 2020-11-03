using BankingApi.Data.Services;
using BankingApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionsController(IInstitutionService service)
        {
            _institutionService = service;
        }

        // GET: api/institutions
        [HttpGet]
        public IActionResult Get() => Ok(_institutionService.GetAll());

        // GET api/institutions/5
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var institution = await _institutionService.GetByIdAsync(id);

            if (institution is null)
            {
                return NotFound();                
            }

            return Ok(institution);
        }

        // POST api/institutions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewInstitutionDto institution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _institutionService.CreateAsync(institution);

            if (result is null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        // PUT api/institutions/5
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] InstitutionDto institution)
        {
            if (!ModelState.IsValid || id != institution.Id)
            {
                return BadRequest();
            }

            if (await _institutionService.UpdateAsync(institution) != 1)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/institutions/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _institutionService.DeleteAsync(id))
            { 
                return BadRequest();
            }

            return NoContent();
        }
    }
}
