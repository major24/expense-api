using System;
using System.Threading.Tasks;

namespace expense_api.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<bool> IsValidUser(string userid, string password);
    }
}
