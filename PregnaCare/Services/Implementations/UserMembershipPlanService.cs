using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserMembershipPlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userMembershipRepository = _unitOfWork.GetRepository<UserMembershipPlan, Guid>();
            _userRepository = _unitOfWork.GetRepository<User, Guid>();
            _membershipRepository = _unitOfWork.GetRepository<MembershipPlan, Guid>();
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
            if(membershipPlan == null)
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
                                                                                x.ExpiryDate < DateTime.Now)).FirstOrDefault();

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
                var durationDays = (request.EndDate - request.StartDate).Days;
                userMembershipPlan.ExpiryDate = userMembershipPlan?.ExpiryDate.Value.AddDays(durationDays);
                _userMembershipRepository.Update(userMembershipPlan);
            }

            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
