using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelviaStoreAPI.Models;
using IntelviaStoreAPI.Services;

namespace IntelviaStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }


        [HttpGet("Users")]
        public async Task<IEnumerable<Register>> GetUser()
        {
            return await _userService.GetUser();
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isRegistered = await _authService.Register(register);
            if (!isRegistered.IsAuthenticated)
            {
                return BadRequest(isRegistered.Message);
            }

            return Ok(isRegistered);
        }

        //[HttpPost("EditUser")]
        //public async Task<IActionResult> EditUser([FromBody] Register register, string id)
        //{
        //    var user = await _
        //}

        [HttpPost("DeleteUser")]
        public async Task<string> DeleteUser(string id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
