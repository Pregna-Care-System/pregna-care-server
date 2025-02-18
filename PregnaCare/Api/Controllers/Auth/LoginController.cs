using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Api.Models.Responses.AuthResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Auth
{
    /// <summary>
    /// LoginController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class LoginController : AbstractApiController<LoginRequest, LoginResponse>
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authService"></param>
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public override async Task<LoginResponse> Exec([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return response;
        }

        [HttpPost("Google")]
        public async Task<LoginResponse> LoginGoogle([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginGoogleAsync(request);
            return response;
        }
    }
}
