using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tulip_API.Contracts;
using Tulip_API.DTOs;

namespace Tulip_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;// Handle sign in and sign out and autherntications
        private readonly UserManager<IdentityUser> _userManager;// Manage user operations
        private readonly ILoggerService _logger;

        public UsersController(SignInManager<IdentityUser> signInManager,  
            UserManager<IdentityUser> userManager,
            ILoggerService logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        /// <summary>
        /// User Login Endpoint
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                var username = userDTO.Username;
                var password = userDTO.Password;
                _logger.LogInfo($"{location}: Login attempt from user {username}");
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInfo($"{location}: {username} successfully authenticated");
                    var user = await _userManager.FindByNameAsync(username);
                    return Ok(user);
                }
                _logger.LogInfo($"{location}: {username} not authenticated");
                return Unauthorized(userDTO);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the administrator");
        }
    }
}
