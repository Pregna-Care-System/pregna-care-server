using Microsoft.AspNetCore.Identity;
using PregnaCare.Api.Controllers.Auth;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authRepository"></param>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        public AuthService(IAuthRepository authRepository, UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = new LoginResponse();
            var detailErrorList = new List<DetailError>();

            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var isSamePassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!isSamePassword)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var roleName = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault();
            if (string.IsNullOrEmpty(roleName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    MessageId = Messages.E00001,
                    Message = Messages.GetMessageById(Messages.E00001)
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

            var accessToken = _tokenService.GenerateToken(identityUser, roleName, TokenTypeEnum.AccessToken.ToString());

            var refreshToken = await _userManager.GetAuthenticationTokenAsync(identityUser, LoginProviderEnum.InternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString());
            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = _tokenService.GenerateToken(identityUser, roleName, TokenTypeEnum.RefreshToken.ToString());

                await _userManager.SetAuthenticationTokenAsync(identityUser, LoginProviderEnum.InternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString(), refreshToken);
            }

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = new Token { RefreshToken = refreshToken, AccessToken = accessToken };
            return response;
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

            var userAccount = new User
            {
                FullName = request.FullName,
                Email = request.Email,
            };

            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.Email.ToUpper(),
            };

            await _authRepository.RegisterAsync(userAccount, identityUser, request.Password, request.RoleName);

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
