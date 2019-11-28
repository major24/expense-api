using System;
using expense_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace expense_api.Repositories
{
    public interface ICostCentreUserApprovalDetailRepository
    {
        Task<IEnumerable<CostCentreUserApprovalDetail>> GetAll();
    }
}
