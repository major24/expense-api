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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User/user11
        [HttpGet("{userid}", Name = "GetByUserId")]
        public async Task<ActionResult<User>> GetByUserId(string userId)
        {
            if (userId == null || userId == "")
            {
                return BadRequest($"Must provide a user id");
            }

            try
            {
                User user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    return NotFound($"User not found for the provided id {userId}");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by id. " + ex.Message);
            }

        }

        // GET: api/users
        [HttpGet()]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users. " + ex.Message);
            }

        }
    }
}
