using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Api.Models.Responses.AuthResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> LoginGoogleAsync(LoginRequest request);
        Task AddTokenAsync(Guid userId, string tokenType, string otp, DateTime expirationTime);
        Task<bool> VerifyAsync(Guid userId, string tokenType, string otp);
        Task RemoveTokenAsync(Guid userId, string tokenType);
    }
}
