using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using expense_api.Models;

namespace expense_api.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<Transaction> GetById(int id);
    }
}
