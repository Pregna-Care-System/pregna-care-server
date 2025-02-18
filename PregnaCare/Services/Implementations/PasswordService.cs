using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Api.Models.Responses.AuthResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly PregnaCareAuthDbContext _authContext;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authContext"></param>
        /// <param name="userManager"></param>
        /// <param name="emailService"></param>
        public PasswordService(PregnaCareAuthDbContext authContext, UserManager<IdentityUser<Guid>> userManager, IEmailService emailService)
        {
            _authContext = authContext;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = new ForgotPasswordResponse();
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (!ValidationUtils.IsValidEmail(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            if (request.Email.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.NewPassword))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.NewPassword),
                    Value = request.NewPassword,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (request.NewPassword.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.NewPassword),
                    Value = request.NewPassword,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (!ValidationUtils.IsValidPassword(request.NewPassword))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.NewPassword),
                    Value = request.NewPassword,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            var user = await _authContext.Set<IdentityUserToken<Guid>>().FirstOrDefaultAsync(x => x.Value == request.Token);
            if (user == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Token),
                    Value = request.Token,
                    Message = Messages.GetMessageById(Messages.E00001),
                    MessageId = Messages.E00001
                });
            }

            if (detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
