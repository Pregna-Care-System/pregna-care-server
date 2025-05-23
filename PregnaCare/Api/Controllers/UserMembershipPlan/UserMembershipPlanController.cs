﻿using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.UserMembershipPlan
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserMembershipPlanController : ControllerBase
    {
        private readonly IUserMembershipPlanSerivce _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serivce"></param>
        public UserMembershipPlanController(IUserMembershipPlanSerivce serivce)
        {
            _service = serivce;
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUserMembershipPlan([FromBody] CreateUserMembershipPlanRequest request)
        {
            var response = await _service.ActivateUserMembershipPlan(request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserMembershipPlan()
        {
            var result = await _service.GetUserMembershipPlanList();
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllUserTransaction(Guid userId)
        {
            var result = await _service.GetUserTransaction(userId);
            return Ok(result);
        }
    }
}
