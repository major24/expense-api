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
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        // POST: api/Authentication
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials.UserId == null || userCredentials.UserId == "" || userCredentials.Password == null || userCredentials.Password == "")
            {
                return BadRequest("Missing required field(s) for user authentication");
            }

            try
            {
                var result =  await _authenticationRepository.IsValidUser(userCredentials.UserId, userCredentials.Password);
                if (result)
                {
                    return Ok("Authentication Success");
                }
                return NotFound("Authentication Failed");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "System error. " + ex.Message);
            }
        }




    }
}
