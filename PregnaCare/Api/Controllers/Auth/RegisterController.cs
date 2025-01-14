using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Common.Api;
using PregnaCare.Core.Models;
using PregnaCare.Services.Interfaces;
using System.Web;

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
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authService"></param>
        public RegisterController(IAuthService authService, IEmailService emailService, UserManager<IdentityUser> userManager)
        {
            _authService = authService;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost]
        public override async Task<RegisterResponse> Exec([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("No user with this email exists");
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedCode = HttpUtility.UrlEncode(code);

            var callbackUrl = Url.Action("ConfirmEmail", "Register", new { userId = user.Id, code = encodedCode }, HttpContext.Request.Scheme);
            Console.WriteLine($"Callback URL: {callbackUrl}");
            if (!_emailService.SendEmail(user.Email, "Confirm your account", $"<a href=\"{callbackUrl}\">Confirm</a>", ""))
            {
                throw new Exception("Email sending failed. Please try again later.");
            }

            return response;
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid confirmation request");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect("http://localhost:3000/login"); 
            }

            return BadRequest("Email confirmation failed");
        }




    }
}
