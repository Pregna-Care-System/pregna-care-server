using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
using PregnaCare.Infrastructure.Data;
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
        private readonly PregnaCareAuthDbContext _authContext;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authContext"></param>
        /// <param name="userManager"></param>
        /// <param name="emailService"></param>
        /// <param name="authService"></param>
        /// <param name="passwordService"></param>
        public PasswordController(PregnaCareAuthDbContext authContext, UserManager<IdentityUser<Guid>> userManager, IEmailService emailService, IAuthService authService, IPasswordService passwordService)
        {
            _authContext = authContext;
            _userManager = userManager;
            _emailService = emailService;
            _authService = authService;
            _passwordService = passwordService;
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _passwordService.ForgotPasswordAsync(request);
            if (!response.Success) return BadRequest(response);

            var user = await _userManager.FindByEmailAsync(request.Email);

            var token = CommonUtils.GenerateOtp();
            var callbackUrl = Url.Action("ResetPasswordPage", "Password", new { token = token }, HttpContext.Request.Scheme);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "PasswordResetRequest.html");
            string emailContent = await System.IO.File.ReadAllTextAsync(path);

            emailContent = emailContent
                .Replace("{FullName}", user.Email)
                .Replace("{ResetPasswordUrl}", callbackUrl);

            if (!_emailService.SendEmail(user.Email, "Reset Your Password", emailContent, ""))
            {
                response.Success = false;
                response.MessageId = Messages.E99999;
                response.Message = Messages.GetMessageById(Messages.E99999);
                return BadRequest(response);
            }

            await _authService.AddTokenAsync(user.Id, TokenTypeEnum.ResetPasswordToken.ToString(), token, DateTime.Now.AddHours(1));
            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _passwordService.ResetPasswordAsync(request);
            if (!response.Success) return BadRequest(response);

            var userToken = await _authContext.Set<IdentityUserToken<Guid>>().FirstOrDefaultAsync(x => x.Value == request.Token && x.Name == TokenTypeEnum.ResetPasswordToken.ToString());
            if (userToken == null || (DateTime?)_authContext.Entry(userToken).Property("ExpirationTime").OriginalValue < DateTime.Now)
            {
                response.Success = false;
                response.MessageId = Messages.E99999;
                response.Message = Messages.GetMessageById(Messages.E99999);
                return BadRequest(response);
            }

            var identityUser = await _userManager.FindByIdAsync(userToken.UserId.ToString());
            if (identityUser == null)
            {
                response.Success = false;
                response.MessageId = Messages.E99999;
                response.Message = Messages.GetMessageById(Messages.E99999);
                return BadRequest(response);
            }

            _ = await _userManager.RemovePasswordAsync(identityUser);
            _ = await _userManager.AddPasswordAsync(identityUser, request.NewPassword);
            await _authService.RemoveTokenAsync(identityUser.Id, TokenTypeEnum.ResetPasswordToken.ToString());
            return Ok(response);
        }

        [HttpGet("ResetPasswordPage")]
        public IActionResult ResetPasswordPage([FromQuery] string token)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "ResetPassword.html");
            return PhysicalFile(filePath, "text/html");
        }
    }
}
