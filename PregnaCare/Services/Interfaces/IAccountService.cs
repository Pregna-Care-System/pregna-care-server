using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Api.Models.Responses.AccountResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountListResponse> GetAllMemberAsync();
        Task<AccountResponse> GetUserById(Guid id);
        Task<AccountResponse> UpdateAccount(Guid id, UpdateAccountRequest request);
    }
}
