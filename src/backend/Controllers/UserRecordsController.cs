using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JK.DAL.Mappings;
using JK.DAL.Models;
using JK.Helpers;
using JK.Services;

namespace JK.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class UserRecordsController: ControllerBase
    {
        private readonly UserRecordService _UserRecordService;
        private readonly IMapper _mapper;
        private IConfiguration _config { get; }

        public UserRecordsController(UserRecordService UserRecordService, IConfiguration config, IMapper automapper)
        {
            _UserRecordService = UserRecordService;
            _config = config;
            _mapper = automapper;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<Response>> GetUserRecords()
        {

            var _UserRecordResponse = await _UserRecordService.GetUserRecords();

            return StatusCode(200, new Response
            {
                status = true,
                message = "UserRecordes were found",
                data = _UserRecordResponse
            });

        }

        [Route("GetUserRecord/{id}")]
        [HttpGet]
        public async Task<ActionResult<Response>> GetUserRecord(Guid id)
        {

            var _UserRecordResponse = await _UserRecordService.GetUserRecord(id);

            return StatusCode(200, new Response
            {
                status = true,
                message = "UserRecord was found",
                data = _UserRecordResponse
            });

        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<Response> GetUserRecordsCount()
        {

            var _UserRecordResponse = _UserRecordService.GetUserRecordsCount();

            return StatusCode(200, new Response
            {
                status = true,
                message = "UserRecord were found",
                data = _UserRecordResponse
            });

        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Response>> CreateUserRecord([FromBody]UserRecordDto UserRecord)
        {

            var payloadUserRecord = UserRecord;
            var UserRecordEntity = _mapper.Map<UserRecord>(payloadUserRecord);
            var _UserRecordResponse = await _UserRecordService.CreateUserRecord(UserRecordEntity);

            if (_UserRecordResponse == null)
            {
                return StatusCode(403, new Response
                {
                    status = false,
                    message = "UserRecord already exists",
                    data = false
                });
            }

            return StatusCode(200, new Response
            {
                status = true,
                message = "UserRecord was successfully created.",
                data = _UserRecordResponse
            });

        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Response>> UpdateUserRecord([FromBody]UserRecordDto UserRecord)
        {
            var payloadUserRecord = UserRecord;
            var UserRecordEntity = _mapper.Map<UserRecord>(payloadUserRecord);
            var updateUserRecord = await _UserRecordService.UpdateUserRecord(UserRecordEntity);

            return StatusCode(200, new Response
            {
                status = true,
                message = "UserRecord was successfully updated",
                data = updateUserRecord
            });

        }
    }
}
