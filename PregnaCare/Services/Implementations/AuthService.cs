using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Controllers.Auth;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly PregnaCareAppDbContext _dbContext;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authRepository"></param>
        /// <param name="tokenService"></param>
        public AuthService(PregnaCareAppDbContext dbContext, IAuthRepository authRepository, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = new LoginResponse();
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
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

            if (!ValidationUtils.IsValidEmail(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
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

            if (request.Password.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Password),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (!ValidationUtils.IsValidPassword(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            var user = await _dbContext.Users.AsNoTracking().ToListAsync();//.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == request.Email);
            var isSamePassword = PasswordUtils.VerifyPassword(request.Password, user?[0].Password ?? "");

            if (user is null || !isSamePassword || detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00003;
                response.Message = Messages.GetMessageById(Messages.E00003);
                response.DetailErrorList = detailErrorList;
                return response;
            }


            var accessToken = _tokenService.GenerateToken(user[0], "", TokenTypeEnum.AccessToken.ToString());
            //var refreshToken = (await _dbContext.JwtTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == user.Id && x.ExpiresAt.Minute >= DateTime.Now.Minute))?.RefreshToken ?? "";

            //if (string.IsNullOrEmpty(refreshToken))
            //{
            //    refreshToken = _tokenService.GenerateToken(user, user?.Role.RoleName, TokenTypeEnum.RefreshToken.ToString()).Substring(0, 255);

            //    var refreshTokenExpiration = Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRATION") ?? "0";

            //    await _dbContext.JwtTokens.AddAsync(new JwtToken
            //    {
            //        UserId = user.Id,
            //        RefreshToken = refreshToken,
            //        ExpiresAt = DateTime.Now.AddDays(double.Parse(refreshTokenExpiration)),
            //    });

            //    await _dbContext.SaveChangesAsync();
            //}

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            //response.Response = new Token { RefreshToken = refreshToken, AccessToken = accessToken };
            return response;
        }

        public async Task<LoginResponse> LoginGoogleAsync(LoginRequest request)
        {
            var response = new LoginResponse();
            var detailErrorList = new List<DetailError>();

            //var identityUser = await _userManager.FindByEmailAsync(request.Email);
            //if (identityUser == null)
            //{
            //    detailErrorList.Add(new DetailError
            //    {
            //        FieldName = nameof(request.Email),
            //        Value = request.Email,
            //        MessageId = Messages.E00002,
            //        Message = Messages.GetMessageById(Messages.E00002)
            //    });
            //}

            //var roleName = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault();
            //if (string.IsNullOrEmpty(roleName))
            //{
            //    detailErrorList.Add(new DetailError
            //    {
            //        FieldName = nameof(request.Password),
            //        Value = request.Password,
            //        MessageId = Messages.E00001,
            //        Message = Messages.GetMessageById(Messages.E00001)
            //    });
            //}
            //var isConfirm = (await _userManager.IsEmailConfirmedAsync(identityUser));
            //if (!isConfirm)
            //{
            //    detailErrorList.Add(new DetailError
            //    {
            //        FieldName = nameof(request.Email),
            //        Value = request.Email,
            //        MessageId = Messages.E00003,
            //        Message = Messages.GetMessageById(Messages.E00003)
            //    });
            //}

            //if (detailErrorList.Any())
            //{
            //    response.Success = false;
            //    response.MessageId = Messages.E00002;
            //    response.Message = Messages.GetMessageById(Messages.E00002);
            //    response.DetailErrorList = detailErrorList;
            //    return response;
            //}
            //var accessToken = _tokenService.GenerateToken(identityUser, roleName, TokenTypeEnum.AccessToken.ToString());

            //var refreshToken = await _userManager.GetAuthenticationTokenAsync(identityUser, LoginProviderEnum.InternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString());
            //if (string.IsNullOrEmpty(refreshToken))
            //{
            //    refreshToken = _tokenService.GenerateToken(identityUser, roleName, TokenTypeEnum.RefreshToken.ToString());

            //    await _userManager.SetAuthenticationTokenAsync(identityUser, LoginProviderEnum.InternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString(), refreshToken);
            //}

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            //response.Response = new Token { RefreshToken = refreshToken, AccessToken = accessToken };
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

            if (request.FullName.Length > 60)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.FullName),
                    Value = request.FullName,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
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

            if (!ValidationUtils.IsValidEmail(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            if (request.Email.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Email),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
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

            if (request.Password.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Password),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (!ValidationUtils.IsValidPassword(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
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

            var role = _dbContext.Roles.AsNoTracking().FirstOrDefault(x => x.RoleName == request.RoleName);
            if (role == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(RegisterRequest.RoleName),
                    Value = request.RoleName,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
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

            var isExist = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
            if (isExist != null)
            {
                response.Success = false;
                response.MessageId = Messages.E00011;
                response.Message = Messages.GetMessageById(Messages.E00011);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var password = PasswordUtils.HashPassword(request.Password);
            var userAccount = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = password,
                
                IsDeleted = false,
            };

            await _authRepository.RegisterAsync(userAccount);

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
