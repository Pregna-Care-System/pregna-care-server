using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Api.Models.Responses.AuthResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Enums;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

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
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authService"></param>
        public RegisterController(IAuthService authService, IEmailService emailService, UserManager<IdentityUser<Guid>> userManager)
        {
            _authService = authService;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost]
        public override async Task<RegisterResponse> Exec([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (!response.Success) return response;

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new Exception("No user with this email exists");

            string code = CommonUtils.GenerateOtp();
            var callbackUrl = Url.Action("ConfirmEmail", "Register", new { userId = user.Id, code = code }, HttpContext.Request.Scheme);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "SignupConfirmation.html");
            string emailContent = await System.IO.File.ReadAllTextAsync(path);

            emailContent = emailContent.Replace("{FullName}", request.FullName).Replace("{ConfirmationUrl}", callbackUrl);
            if (!_emailService.SendEmail(user.Email, "Confirm your account", emailContent, ""))
            {
                throw new Exception("Email sending failed. Please try again later.");
            }

            await _authService.AddTokenAsync(user.Id, TokenTypeEnum.OTP.ToString(), code, DateTime.Now.AddYears(1));
            return response;
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid confirmation request.");
            }

            _ = HttpUtility.UrlDecode(code);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var result = await _authService.VerifyAsync(user.Id, TokenTypeEnum.OTP.ToString(), code);
            if (result)
            {
                user.EmailConfirmed = true;
                await _authService.RemoveTokenAsync(user.Id, TokenTypeEnum.OTP.ToString());
                return Redirect("http://14.225.205.143:3000/email-success-confirm");
            }

            return BadRequest("OTP has expired.");
        }

    }
}
