using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Api.Models.Responses.AccountResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountListResponse> GetAllMemberAsync(string filterType = null, string name = null);
        Task<AccountResponse> GetUserById(Guid id);
        Task<AccountResponse> UpdateAccount(Guid id, UpdateAccountRequest request);
    }
}
