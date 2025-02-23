using PregnaCare.Api.Models.Requests.AuthRequestModel;
using PregnaCare.Api.Models.Responses.AuthResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
