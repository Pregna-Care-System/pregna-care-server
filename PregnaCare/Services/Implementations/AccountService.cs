using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        public AccountService(IAccountRepository repository)
        {
            _repo = repository;
        }

        public async Task<AccountListResponse> GetAllMemberAsync()
        {
            var users = await _repo.GetAllAsync();
            var accountList = users.Select(user => Mapper.MapToAccountDTO(user)).ToList();

            return new AccountListResponse
            {
                Success = true,
                Response = accountList
            };
        }

        public Task<AccountResponse> GetMemberById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAccount(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
