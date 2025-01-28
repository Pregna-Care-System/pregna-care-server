using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
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
        private readonly PregnaCareAuthDbContext _authContext;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="authRepository"></param>
        /// <param name="tokenService"></param>
        /// <param name="userManager"></param>
        public AuthService(PregnaCareAppDbContext dbContext, PregnaCareAuthDbContext authContext, IAuthRepository authRepository, ITokenService tokenService, UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _dbContext = dbContext;
            _authContext = authContext;
            _authRepository = authRepository;
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddTokenAsync(Guid userId, string tokenType, string otp, DateTime expirationTime)
        {
            var existingToken = await _authContext.Set<IdentityUserToken<Guid>>()
                                                  .FirstOrDefaultAsync(t =>
                                                                            t.UserId == userId &&
                                                                            t.LoginProvider == LoginProviderEnum.InternalProvider.ToString() &&
                                                                            t.Name == tokenType);

            if (existingToken != null)
            {
                _authContext.Set<IdentityUserToken<Guid>>().Remove(existingToken);
                await _authContext.SaveChangesAsync();
            }

            var token = new IdentityUserToken<Guid>
            {
                UserId = userId,
                LoginProvider = LoginProviderEnum.InternalProvider.ToString(),
                Name = TokenTypeEnum.OTP.ToString(),
                Value = otp,
            };

            _authContext.Set<IdentityUserToken<Guid>>().Add(token);
            _authContext.Entry(token).Property("ExpirationTime").CurrentValue = expirationTime;
            await _authContext.SaveChangesAsync();
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

            if (string.IsNullOrEmpty(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (request.Password.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Email,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (!ValidationUtils.IsValidPassword(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email);
            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            var isSamePassword = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, request.Password) == PasswordVerificationResult.Success;

            if (user is null || !isSamePassword || detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00003;
                response.Message = Messages.GetMessageById(Messages.E00003);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var userRole = await _dbContext.UserRoles.AsNoTracking().Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == user.Id);
            var accessToken = _tokenService.GenerateToken(user, userRole.Role.RoleName, TokenTypeEnum.AccessToken.ToString());
            var tokenObject = await _authContext.Set<IdentityUserToken<Guid>>()
                                                  .FirstOrDefaultAsync(t =>
                                                                            t.UserId == identityUser.Id &&
                                                                            t.LoginProvider == LoginProviderEnum.InternalProvider.ToString() &&
                                                                            t.Name == TokenTypeEnum.RefreshToken.ToString());

            string responseRefreshToken = _tokenService.GenerateToken(user, userRole.Role.RoleName, TokenTypeEnum.RefreshToken.ToString()).Substring(100);
            var refreshTokenExpiration = Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRATION") ?? "0";
            if (tokenObject != null)
            {
                var expirationTime = (DateTime?)_authContext?.Entry(tokenObject).Property("ExpirationTime").OriginalValue;
                if (expirationTime.HasValue && expirationTime.Value < DateTime.Now)
                {
                    tokenObject.Value = responseRefreshToken;
                    _authContext.Entry(tokenObject).Property("ExpirationTime").CurrentValue = DateTime.Now.AddDays(double.Parse(refreshTokenExpiration));
                }
            }
            else
            {
                tokenObject = new IdentityUserToken<Guid>
                {
                    UserId = identityUser.Id,
                    LoginProvider = LoginProviderEnum.InternalProvider.ToString(),
                    Name = TokenTypeEnum.RefreshToken.ToString(),
                    Value = responseRefreshToken,
                };
                _authContext.Set<IdentityUserToken<Guid>>().Add(tokenObject);
                _authContext.Entry(tokenObject).Property("ExpirationTime").CurrentValue = DateTime.Now.AddDays(double.Parse(refreshTokenExpiration));

            }

            await _dbContext.SaveChangesAsync();
            await _authContext.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = new Token { RefreshToken = tokenObject.Value, AccessToken = accessToken };
            return response;
        }

        public async Task<LoginResponse> LoginGoogleAsync(LoginRequest request)
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
            var isConfirm = (await _userManager.IsEmailConfirmedAsync(identityUser));
            if (!isConfirm)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    MessageId = Messages.E00003,
                    Message = Messages.GetMessageById(Messages.E00003)
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

            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email);
            var accessToken = _tokenService.GenerateToken(user, roleName, TokenTypeEnum.AccessToken.ToString());

            var refreshToken = await _userManager.GetAuthenticationTokenAsync(identityUser, LoginProviderEnum.ExternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString());
            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = _tokenService.GenerateToken(user, roleName, TokenTypeEnum.RefreshToken.ToString());

                await _userManager.SetAuthenticationTokenAsync(identityUser, LoginProviderEnum.ExternalProvider.ToString(), TokenTypeEnum.RefreshToken.ToString(), refreshToken);
            }

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
                    FieldName = nameof(request.FullName),
                    Value = request.FullName,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (request.FullName.Length > 60)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.FullName),
                    Value = request.FullName,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

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

            if (string.IsNullOrEmpty(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00005),
                    MessageId = Messages.E00005
                });
            }

            if (request.Password.Length > 40)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00007),
                    MessageId = Messages.E00007
                });
            }

            if (!ValidationUtils.IsValidPassword(request.Password))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Password),
                    Value = request.Password,
                    Message = Messages.GetMessageById(Messages.E00009),
                    MessageId = Messages.E00009
                });
            }

            if (string.IsNullOrEmpty(request.RoleName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.RoleName),
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
                    FieldName = nameof(request.RoleName),
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

            await _userManager.CreateAsync(new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                UserName = request.Email,
                NormalizedUserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email,
            }, request.Password);

            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            await _userManager.AddToRoleAsync(identityUser, request.RoleName);

            var userAccount = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = identityUser.PasswordHash ?? string.Empty,
                IsDeleted = false,
            };

            var userRole = new UserRole
            {
                RoleId = role.Id,
                UserId = userAccount.Id,
            };

            userAccount.UserRoles.Add(userRole);
            await _authRepository.RegisterAsync(userAccount);

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task RemoveTokenAsync(Guid userId, string tokenType)
        {
            var token = await _authContext.Set<IdentityUserToken<Guid>>()
                                          .FirstOrDefaultAsync(t => t.UserId == userId &&
                                                                    t.LoginProvider == LoginProviderEnum.InternalProvider.ToString() &&
                                                                    t.Name == tokenType);

            if (token != null)
            {
                _authContext.Set<IdentityUserToken<Guid>>().Remove(token);
                await _authContext.SaveChangesAsync();
            }
        }

        public async Task<bool> VerifyAsync(Guid userId, string tokenType, string otp)
        {
            var token = await _authContext.Set<IdentityUserToken<Guid>>()
                                          .FirstOrDefaultAsync(t => t.UserId == userId
                                                                    && t.LoginProvider == LoginProviderEnum.InternalProvider.ToString()
                                                                    && t.Name == tokenType);

            if (token == null || token.Value != otp) return false;

            var expirationTime = (DateTime?)_authContext.Entry(token).Property("ExpirationTime").OriginalValue;
            if (expirationTime.HasValue && expirationTime.Value < DateTime.Now) return false;

            return true;
        }
    }
}
