using System;
using System.Threading.Tasks;
using expense_api.Models;

namespace expense_api.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> GetById(int id);
        Task<ExpenseReport> GetByIdForReport(int id);
        Task<int> Save(Expense expense);
    }
}
