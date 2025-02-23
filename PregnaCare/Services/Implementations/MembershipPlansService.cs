using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class MembershipPlansService : IMembershipPlansService
    {
        private readonly IMembershipPlansRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public MembershipPlansService(IMembershipPlansRepository membershipPlansRepository, IUnitOfWork unitOfWork)
        {
            _repo = membershipPlansRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MembershipPlanResponse> AddPlanAsync(MembershipPlanRequest request, List<Guid> featureIds)
        {
            var response = new MembershipPlanResponse();
            var detailErrorList = new List<DetailError>();

            // Validate Plan Name
            if (string.IsNullOrEmpty(request.PlanName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.PlanName),
                    Value = request.PlanName,
                    Message = "Plan name is required",
                    MessageId = "E00005"
                });
            }

            // Validate Price
            if (request.Price <= 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Price),
                    Value = request.Price.ToString(),
                    Message = "Price must be greater than zero",
                    MessageId = "E00002"
                });
            }

            var plan = Mapper.MapToMembershipPlan(request);

            plan.Id = Guid.NewGuid();
            plan.IsDeleted = false;
            plan.CreatedAt = DateTime.UtcNow;
            plan.UpdatedAt = DateTime.UtcNow;

            await _repo.AddPlanAsync(plan, featureIds);

            var addPlan = await _repo.GetPlanById(plan.Id);

            response.Response = addPlan;
            response.Success = true;
            response.Message = "Plan added successfully";

            return response;
        }

        public async Task<MembershipPlanResponse> DeletePlanAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            if (plan == null)
            {
                _ = new MembershipPlanResponse
                {
                    Success = false,
                    Message = "Plan not found",
                    MessageId = "E00004"
                };
            }

            await _repo.DeletePlanAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return new MembershipPlanResponse
            {
                Success = true,
                Message = "Delete plan successfully"
            };

        }


        public async Task<MembershipPlanResponse> GetPlanByIdAsync(Guid id)
        {
            var plan = await _repo.GetPlanById(id);
            return new MembershipPlanResponse
            {
                Success = true,
                Response = plan
            };
        }

        public async Task<MembershipPlanResponse> GetPlanByNameAsync(string name)
        {
            var plan = await _repo.GetPlanByName(name);
            return new MembershipPlanResponse
            {
                Success = true,
                Response = plan
            };
        }

        public async Task<MembershipPlanListResponse> GetPlanWithFeatureAsync()
        {
            var planFeature = await _repo.GetPlansWithFeaturesAsync();
            return new MembershipPlanListResponse
            {
                Success = true,
                Response = planFeature
            };
        }

        public async Task<MembershipPlanResponse> UpdatePlanAsync(Guid id, MembershipPlanRequest request, List<Guid> featureIds)
        {
            var existingPlan = await _repo.GetByIdAsync(id);
            if (existingPlan == null)
            {
                _ = new MembershipPlanResponse
                {
                    Success = false,
                    Message = "Plan not found",
                    MessageId = "E00004"
                };
            }

            existingPlan.PlanName = request.PlanName;
            existingPlan.Price = request.Price;
            existingPlan.Description = request.Description;
            existingPlan.Duration = request.Duration;
            existingPlan.UpdatedAt = DateTime.Now;

            await _repo.Update(existingPlan, featureIds);
            await _unitOfWork.SaveChangesAsync();

            var addPlan = await _repo.GetPlanById(id);

            return new MembershipPlanResponse
            {
                Response = addPlan,
                Success = true,
                Message = "Updated successfully"
            };
        }
    }
}
