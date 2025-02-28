﻿using Microsoft.AspNetCore.Mvc;
using PregnaCare.Services.Interfaces;
using PregnaCare.Api.Models.Requests.AccountRequestModel;

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
        public async Task<IActionResult> GetAllMember()
        {
            var result = await _accountService.GetAllMemberAsync();
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
