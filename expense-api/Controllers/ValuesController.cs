using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace expense_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        // public ActionResult<IEnumerable<string>> Get()
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Invalid request for the id {id}");
            }
            return Content($"Value submitted: {id}");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST api/values
        [HttpPost]
        public IActionResult StartBatchJob()
        {
            return Ok("Batch job started.");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
