using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Account
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMember()
        {
            var result = await _accountService.GetAllMemberAsync();
            return Ok(result);
        }
    }
}
