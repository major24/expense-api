using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using expense_api.Models;
using expense_api.Repositories;

namespace expense_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<Transaction>> Get()
        {
            return await _transactionRepository.GetAll();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Transaction>> Get(int id)
        {
            if(id < 1)
            {
                return BadRequest($"Invalid request for the id {id}");
            }

            Transaction transaction = await _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return NotFound($"Transaction not found for the provided id {id}");
            }
            return transaction;
        }

        // POST: api/Transactions
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
