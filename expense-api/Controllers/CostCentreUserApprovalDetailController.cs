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
    [ApiController]
    public class CostCentreUserApprovalDetailController : ControllerBase
    {
        private readonly ICostCentreUserApprovalDetailRepository _costCentreUserApprovalDetailRepository;

        public CostCentreUserApprovalDetailController(ICostCentreUserApprovalDetailRepository costCentreUserApprovalDetailRepository)
        {
            _costCentreUserApprovalDetailRepository = costCentreUserApprovalDetailRepository;
        }

        [Route("api/costcentres/users/approvals")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostCentreUserApprovalDetail>>> GetAll()
        {
            IEnumerable<CostCentreUserApprovalDetail> approvals = await _costCentreUserApprovalDetailRepository.GetAll();
            if (approvals == null)
            {
                return NotFound("Costcentre user approvals not found");
            }
            return approvals.ToArray();
        }

    }
}
