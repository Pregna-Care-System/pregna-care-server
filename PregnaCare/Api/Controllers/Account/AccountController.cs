﻿using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Account
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMember([FromQuery] string filterType = null, [FromQuery] string name = null)
        {
            var result = await _accountService.GetAllMemberAsync(filterType, name);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMemberWithPLanDetail(Guid userId)
        {
            var result = await _accountService.GetMemberInforWithPlanDetail(userId);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet("CurrentUser/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
        {
            var result = await _accountService.GetUserById(userId);
            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserInformation(Guid id, [FromBody] UpdateAccountRequest request)
        {
            var result = await _accountService.UpdateAccount(id, request);
            return Ok(result);
        }
    }
}
