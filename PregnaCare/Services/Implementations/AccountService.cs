using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Api.Models.Responses.AccountResponseModel;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IUnitOfWork _unit;

        public AccountService(IAccountRepository repository, IUnitOfWork unitOfWork)
        {
            _repo = repository;
            _unit = unitOfWork;
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

        public async Task<AccountResponse> GetUserById(Guid id)
        {
            var existingAccount = await _repo.GetByIdAsync(id);
            if (existingAccount == null)
            {
                _ = new AccountResponse
                {
                    Success = false,
                    Message = "Plan not found",
                    MessageId = "E00004"
                };
            }
            var accountDTO = Mapper.MapToAccountDTO(existingAccount);
            return new AccountResponse
            {
                Success = true,
                Response = accountDTO
            };
        }

        public async Task<AccountResponse> UpdateAccount(Guid id, UpdateAccountRequest request)
        {
            var existingAccount = await _repo.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return new AccountResponse
                {
                    Success = false,
                    Message = "User not found",
                    MessageId = "E00004"
                };
            }

            existingAccount.FullName = request.FullName;
            existingAccount.DateOfBirth = request.DateOfBirth;
            existingAccount.PhoneNumber = request.PhoneNumber;
            existingAccount.Address = request.Address;
            existingAccount.Gender = request.Gender;
            existingAccount.ImageUrl = request.ImageUrl;

            _repo.Update(existingAccount);
            await _unit.SaveChangesAsync();

            var accountDTO = Mapper.MapToAccountDTO(existingAccount);

            return new AccountResponse
            {
                Success = true,
                Response = accountDTO
            };
        }
    }
}
