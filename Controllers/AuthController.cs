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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auth.Register(register);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auth.Login(login);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        [HttpPost("AssigningUsers")]
        public async Task<IActionResult> AssigningUserToRole([FromBody] UserToRole userToRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assign = await _auth.UserToRoleAssign(userToRole);

            if (!string.IsNullOrEmpty(assign))
            {
                return BadRequest(assign);
            }

            return Ok(userToRole);

        }
    }
}
