using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        // GET: api/<InstitutionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InstitutionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InstitutionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InstitutionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InstitutionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
