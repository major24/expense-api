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
    public class CostCentresController : ControllerBase
    {
        private readonly ICostCentreRepository _costCentreRepository;

        public CostCentresController(ICostCentreRepository costCentreRepository)
        {
            _costCentreRepository = costCentreRepository;
        }

        // GET: api/CostCentres
        [HttpGet]
        public async Task<IEnumerable<CostCentre>> GetAllCostCentres()
        {
            try
            {
                return await _costCentreRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting cost centres. " + ex.Message);
            }

        }

    }
}
