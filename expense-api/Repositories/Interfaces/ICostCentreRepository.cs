using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using expense_api.Models;

namespace expense_api.Repositories
{
    public interface ICostCentreRepository
    {
        Task<IEnumerable<CostCentre>> GetAll();
    }
}
