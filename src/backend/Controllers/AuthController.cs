using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using JK.DAL.Models;
using JK.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using JK.Helpers;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace JK.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Response>> CreateUser([FromBody]User admin)
        {
            _logger.LogInformation($"----JK: Creating user with email {admin.Email}");
            var _userRegResponse = await _authService.CreateUser(admin);

            if (_userRegResponse == null)
            {
                return StatusCode(403, new Response
                {
                    status = false,
                    message = "Admin User with the same email address or username is already registered. Try using another email account",
                    data = false
                });
            }
            _userRegResponse.Password = null;
            return StatusCode(200, new Response
            {
                status = true,
                message = "User was successfully created, You can now log in with your account",
                data = _userRegResponse
            });

        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> Login([FromBody]LoginUser admin)
        {
            if (string.IsNullOrEmpty(admin.email) || string.IsNullOrEmpty(admin.password))
            {
                return StatusCode(403, new Response
                {
                    status = false,
                    message = "email and password are required for Logging in"
                });
            }
            //get authentication response from service
            var _userCredsCorrect = await _authService.LoginUser(admin.email, admin.password);
            if (_userCredsCorrect != null)
            {

                return StatusCode(200, new Response
                {
                    status = true,
                    message = "Admin User was succesfully authenticated",
                    data = _userCredsCorrect
                });

            }
            return StatusCode(403, new Response
            {
                status = false,
                message = "username and password combination are incorrect"
            });

        }

    }


public class LoginUser
    {
        public string email { get; set; }
        public string password { get; set; }
    }

}

