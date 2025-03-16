using PregnaCare.Api.Models.Requests.AccountRequestModel;
using PregnaCare.Api.Models.Responses.AccountResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

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

        public async Task<AccountListResponse> GetAllMemberAsync(string filterType = null, string name = null)
        {
            var users = await _repo.GetMembers(filterType, name);
            if (users == null)
            {
                return new AccountListResponse
                {
                    Success = false,
                    Message = "Cannot find any members"
                };
            }
            return new AccountListResponse
            {
                Success = true,
                Response = users
            };
        }

        public async Task<AccountResponse> GetMemberInforWithPlanDetail(Guid userId)
        {
            var user = await _repo.GetMemberInforWithPlanDetail(userId);
            if (user == null)
            {
                return new AccountResponse
                {
                    Success = false,
                    Message = "Cannt find member"
                };
            }
            return new AccountResponse
            {
                Success = true,
                Response = user
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
            _ = new AccountResponse();
            var detailErrorList = new List<DetailError>();

            if (request == null)
            {
                return new AccountResponse
                {
                    Success = false,
                    Message = "Invalid request data",
                    MessageId = "E00005"
                };
            }

            var existingAccount = await _repo.GetByIdAsync(id);
            if (existingAccount == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    Message = "User not found",
                    MessageId = "E00004"
                });

                return new AccountResponse
                {
                    Success = false,
                    MessageId = "E00004",
                    Message = "User not found",
                    DetailErrorList = detailErrorList
                };
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.FullName),
                    Value = request.FullName,
                    Message = "Full name cannot be empty",
                });
            }

            if (!ValidationUtils.IsValidPhone(request.PhoneNumber))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.PhoneNumber),
                    Value = request.PhoneNumber,
                    Message = "Phone format is not correct",
                });
            }

            // Kiểm tra tuổi có nhỏ hơn 18 không
            if (request.DateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - request.DateOfBirth.Value.Year;

                if (age < 18)
                {
                    detailErrorList.Add(new DetailError
                    {
                        FieldName = nameof(request.DateOfBirth),
                        Value = request.DateOfBirth.Value.ToString("yyyy-MM-dd"),
                        Message = "User must be at least 18 years old",
                    });
                }
            }

            if (detailErrorList.Any())
            {
                return new AccountResponse
                {
                    Success = false,
                    Message = "Validation failed",
                    MessageId = "E00009",
                    DetailErrorList = detailErrorList
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
