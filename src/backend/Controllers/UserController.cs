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
using Microsoft.Extensions.Configuration;
using JK.DAL.Mappings;
using AutoMapper;
using Newtonsoft.Json;

namespace JK.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private IConfiguration _config { get; }

        public UserController(UserService userService, IConfiguration config, IMapper automapper)
        {
            _userService = userService;
            _config = config;
            _mapper = automapper;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<Response>> GetUsers()
        {

            var _usersResponse = await _userService.GetUsers();

            return StatusCode(200, new Response
            {
                status = true,
                message = "Users were found",
                data = _usersResponse
            });

        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<Response> GetUsersCount()
        {

            var _usersResponse = _userService.GetUsersCount();

            return StatusCode(200, new Response
            {
                status = true,
                message = "Users were found",
                data = _usersResponse
            });

        }

        [Route("GetUser/{id}")]
        [HttpGet]
        public async Task<ActionResult<Response>> GetUser(Guid id)
        {

            var _usersResponse = await _userService.GetUser(id);

            return StatusCode(200, new Response
            {
                status = true,
                message = "Admin User was found",
                data = _usersResponse
            });

        }

        [Route("[action]/{id}")]
        [HttpPost]
        public async Task<ActionResult<Response>> UpdateUser(Guid id,[FromBody]UserDto admin)
        {
            var payloadUser = admin;
            var adminEntity = _mapper.Map<User>(payloadUser);
            adminEntity.Id = id;
            var updateUser = await _userService.UpdateUser(adminEntity);

            return StatusCode(200, new Response
            {
                status = true,
                message = "User Profile was successfully updated",
                data = updateUser
            });

        }

    }

}

