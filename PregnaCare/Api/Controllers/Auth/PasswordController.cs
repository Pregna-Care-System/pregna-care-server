using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Api.Controllers.Auth
{
    /// <summary>
    /// PasswordController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="emailService"></param>
        /// <param name="authService"></param>
        public PasswordController(UserManager<IdentityUser<Guid>> userManager, IEmailService emailService, IAuthService authService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _authService = authService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { success = false, message = "Email not found." });
            }

            var token = CommonUtils.GenerateOtp();
            var callbackUrl = Url.Action("ResetPassword", "Password", new { token = token }, HttpContext.Request.Scheme);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "PasswordResetRequest.html");
            string emailContent = await System.IO.File.ReadAllTextAsync(path);

            emailContent = emailContent
                .Replace("{FullName}", user.Email)
                .Replace("{ResetPasswordUrl}", callbackUrl);

            if (!_emailService.SendEmail(user.Email, "Reset Your Password", emailContent, ""))
            {
                throw new Exception("Email sending failed. Please try again later.");
            }

            await _authService.AddOtpTokenAsync(user.Id, token, DateTime.Now.AddHours(1));
            return Ok(new { success = true, message = "Password reset link has been sent to your email." });
        }

    }
}
