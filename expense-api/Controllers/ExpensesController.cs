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
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpensesController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<ActionResult<GenericResponse>> Post([FromBody] Expense expense)
        {
            try
            { //UserId = "user1", CostCentre = "ITECH", ApproverId = "user5", Status = "Submitted"
                if (expense.UserId == null || expense.UserId == "" || expense.ApproverId == null || expense.ApproverId == "" || expense.CostCentre == null || expense.CostCentre == "")
                {
                    return BadRequest("Missing required field(s) for expense request");
                }
                if(expense.ExpenseItems == null || expense.ExpenseItems.Length < 1)
                {
                    return BadRequest("Should submit atleast one expense item");
                }

                var result = await _expenseRepository.Save(expense);
                if (result > 0)
                {
                    // return Created($"/expenses/{result}", $"Expense has been created: Your Expense ID: {result}");
                    GenericResponse genericResponse = new GenericResponse()
                    {
                        Id = result,
                        Message = "Expense has been created",
                        Status = 201
                    };
                    return genericResponse;
                }
                return BadRequest("Expense submission has failed");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error Saving record. " + ex.Message);
            }
        }

        // public async Task<IEnumerable<Transaction>> Get()
        // public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(string userId)
        // GET: api/Expenses/{expenseid}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseReport>> Get(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Invalid request for the expense id {id}");
            }

            try
            {
                ExpenseReport expenseReport = await _expenseRepository.GetByIdForReport(id);
                if (expenseReport == null)
                {
                    return NotFound($"Expense(s) not found for the provided id {id}");
                }
                return expenseReport;

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving expenses. " + ex.Message);
            }
        }


    }
}
