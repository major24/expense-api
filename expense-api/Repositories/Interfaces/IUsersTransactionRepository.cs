using expense_api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace expense_api.Repositories
{
    public interface IUsersTransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserId(string userId);
    }
}
