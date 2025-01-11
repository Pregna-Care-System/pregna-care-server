using Microsoft.AspNetCore.Identity;
using PregnaCare.Api.Controllers.Auth;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authRepository"></param>
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var response = new RegisterResponse();
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.FullName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.FullName),
                    Value = request.FullName,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (string.IsNullOrEmpty(request.RoleName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.RoleName),
                    Value = request.RoleName,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
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

            var userAccount = new UserAccount
            {
                FullName = request.FullName,
                UserName = request.Email,
            };

            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.Email.ToUpper(),
            };

            await _authRepository.RegisterAsync(userAccount, identityUser, request.Password, request.RoleName);

            response.Success = true;
            return response;
        }
    }
}
