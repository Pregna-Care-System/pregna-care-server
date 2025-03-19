using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Api.Models.Responses.AccountResponseModel;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountListResponse> GetAllMemberAsync(string filterType = null, string name = null);
        Task<AccountResponse> GetUserById(Guid id);
        Task<AccountResponse> UpdateAccount(Guid id, UpdateAccountRequest request);
        Task<AccountResponse> GetMemberInforWithPlanDetail(Guid userId);
    }
}
