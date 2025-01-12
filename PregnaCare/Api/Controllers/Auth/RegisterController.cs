using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Common.Api;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Auth
{
    /// <summary>
    /// RegisterController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class RegisterController : AbstractApiController<RegisterRequest, RegisterResponse>
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authService"></param>
        public RegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public override async Task<RegisterResponse> Exec([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return response;
        }

    }
}
