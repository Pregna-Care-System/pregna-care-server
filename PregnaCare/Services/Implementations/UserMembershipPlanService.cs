using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class UserMembershipPlanService : IUserMembershipPlanSerivce
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserMembershipPlan, Guid> _userMembershipRepository;
        private readonly IGenericRepository<User, Guid> _userRepository;
        private readonly IGenericRepository<MembershipPlan, Guid> _membershipRepository;
        private readonly IUserMembershipPlanRepository _userMembershipPlanRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserMembershipPlanService(IUnitOfWork unitOfWork, IUserMembershipPlanRepository userMembershipPlanRepository)
        {
            _unitOfWork = unitOfWork;
            _userMembershipRepository = _unitOfWork.GetRepository<UserMembershipPlan, Guid>();
            _userRepository = _unitOfWork.GetRepository<User, Guid>();
            _membershipRepository = _unitOfWork.GetRepository<MembershipPlan, Guid>();
            _userMembershipPlanRepository = userMembershipPlanRepository;
        }

        public async Task<CreateUserMembershipPlanResponse> ActivateUserMembershipPlan(CreateUserMembershipPlanRequest request)
        {
            var response = new CreateUserMembershipPlanResponse { Success = false };
            var detailErrorList = new List<DetailError>();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UserId),
                    Value = request.UserId.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var membershipPlan = await _membershipRepository.GetByIdAsync(request.MembershipPlanId);
            if (membershipPlan == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.MembershipPlanId),
                    Value = request.MembershipPlanId.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Any())
            {
                response.MessageId = Messages.E00010;
                response.Message = Messages.GetMessageById(Messages.E00010);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var userMembershipPlan = (await _userMembershipRepository.FindAsync(x => x.UserId == request.UserId &&
                                                                                x.MembershipPlanId == request.MembershipPlanId &&
                                                                                x.IsActive == false)).FirstOrDefault();

            if (userMembershipPlan == null)
            {
                userMembershipPlan = new UserMembershipPlan
                {
                    UserId = request.UserId,
                    MembershipPlanId = request.MembershipPlanId,
                    ActivatedAt = request.StartDate,
                    ExpiryDate = request.EndDate,
                    Price = membershipPlan.Price,
                    IsActive = true
                };

                await _userMembershipRepository.AddAsync(userMembershipPlan);
            }
            else
            {
                userMembershipPlan.IsActive = true;
                userMembershipPlan.Price += membershipPlan.Price;
                if (userMembershipPlan.ActivatedAt == null || userMembershipPlan.ExpiryDate == null)
                {
                    userMembershipPlan.ActivatedAt = request.StartDate;
                    userMembershipPlan.ExpiryDate = request.EndDate;
                }
                else
                {
                    var durationDays = (request.EndDate - request.StartDate).Days;
                    userMembershipPlan.ExpiryDate = userMembershipPlan.ExpiryDate.Value.AddDays(durationDays);
                }

                _userMembershipRepository.Update(userMembershipPlan);
            }

            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<UserMembershipPlanListResponse> GetUserMembershipPlanList()
        {
            var response = await _userMembershipPlanRepository.GetUserMembershipPlanList();
            return new UserMembershipPlanListResponse
            {
                Success = true,
                Response = response
            };
        }

        public async Task<UserMembershipPlanListResponse> GetUserTransaction(Guid userId)
        {
            var transactions = await _userMembershipPlanRepository.GetUserTransactions(userId);

            return new UserMembershipPlanListResponse
            {
                Success = true,
                Response = transactions
            };
        }

        public async Task<UserMembershipPlanListResponse> GetExpiringUserMembershipPlans()
        {
            var tomorrow = DateTime.Now.AddDays(1).Date;
            var expiringPlans = await _userMembershipRepository.FindAsync(p =>
                p.ExpiryDate.HasValue && p.ExpiryDate.Value.Date == tomorrow);


            var expiringPlanDTOs = expiringPlans.Select(p => new UserMembershipPlanDTO
            {
                Id = p.Id,
                UserId = p.UserId,
                MembershipPlanId = p.MembershipPlanId,
                ActivatedAt = p.ActivatedAt,
                ExpiryDate = p.ExpiryDate,
                Price = p.Price,
                IsActive = p.IsActive
            }).ToList();

            return new UserMembershipPlanListResponse
            {
                Success = true,
                Response = expiringPlanDTOs
            };
        }



    }

}
