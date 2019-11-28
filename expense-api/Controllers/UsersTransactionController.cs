using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using expense_api.Models;
using expense_api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace expense_api.Controllers
{
    [Route("api/users/{userid}/transactions")]
    [ApiController]
    public class UsersTransactionController : ControllerBase
    {
        private readonly IUsersTransactionRepository _usersTransactionRepository;

        public UsersTransactionController(IUsersTransactionRepository usersTransactionRepository)
        {
            _usersTransactionRepository = usersTransactionRepository;
        }

        // GET api/users/{user1}/transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(string userId)
        {
            IEnumerable<Transaction> transactions = await _usersTransactionRepository.GetTransactionsByUserId(userId);
            if (transactions == null || transactions.Count() == 0)
            {
                return NotFound("Transaction not found for the provided id.");
            }
            return transactions.ToArray();
        }


    }
}
