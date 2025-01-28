using Microsoft.AspNetCore.Identity;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="emailService"></param>
        public PasswordService(UserManager<IdentityUser<Guid>> userManager, IEmailService emailService)
        {
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
                    FieldName = nameof(RegisterRequest.Email),
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
            return response;
        }

        public Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
