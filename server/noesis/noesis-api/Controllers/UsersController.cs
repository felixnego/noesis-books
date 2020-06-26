using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using noesis_api.Services;
using noesis_api.Models;
using noesis_api.Helpers;



namespace noesis_api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null) return BadRequest(new { message = "Username or password is invalid!" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (_userService.UsernameExists(user.Username)) return BadRequest("Username already taken!");

            user.Password = HashUtils.GetHashString(user.Password);
            await _userService.Register(user);

            if (await _userService.SaveAll()) return Ok(user);

            return BadRequest("Unable to register user!");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();

            return Ok(users);
        }
    }
}