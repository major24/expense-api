using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace expense_api.Models
{
    public class CostCentreUserApprovalDetail
    {
        public CostCentre CostCentre { get; set; }
        public CostCentreUserApproval[] Approvals { get; set; }
    }
}
