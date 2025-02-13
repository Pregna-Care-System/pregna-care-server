using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountListResponse> GetAllMemberAsync();
        Task<AccountResponse> GetMemberById(Guid id);
        Task UpdateAccount(Guid id);
    }
}
