using PregnaCare.Api.Controllers.Auth;

namespace PregnaCare.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> LoginGoogleAsync(LoginRequest request);
        Task AddOtpTokenAsync(Guid userId, string otp, DateTime expirationTime);
        Task<bool> VerifyOtpAsync(Guid userId, string otp);
        Task RemoveOtpTokenAsync(Guid userId);
    }
}
