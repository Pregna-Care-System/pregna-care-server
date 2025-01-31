using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class MembershipPlansService : IMembershipPlansService
    {
        private readonly IMembershipPlansRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public MembershipPlansService (IMembershipPlansRepository membershipPlansRepository, IUnitOfWork unitOfWork)
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
            response.Success = true;
            response.Message = "Plan added successfully";

            return response;
        }

        public async Task<MembershipPlanResponse> DeletePlanAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            if(plan == null)
            {
                new MembershipPlanResponse
                {
                    Success = false,
                    Message = "Plan not found",
                    MessageId = "E00004"
                };
            }

            plan.IsDeleted = true;
            _repo.Update(plan);
            await _unitOfWork.SaveChangesAsync();

            return new MembershipPlanResponse
            {
                Success = true,
                Message = "Delete plan successfully"
            };
            
        }

        public async Task<MembershipPlanListResponse> GetAllPlansAsync()
        {

            var plan = await _repo.GetActivePlanAsync();
            return new MembershipPlanListResponse
            {
                Success = true,
                Message = "All plan received successfully",
                Response = plan,
            };
        }

        public async Task<MembershipPlanResponse> GetPlanByIdAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            return new MembershipPlanResponse
            {
                Success = true,
                Response = plan
            };
        }

        public async Task<MembershipPlanResponse> UpdatePlanAsync(Guid id, MembershipPlanRequest request)
        {
            var existingPlan = await _repo.GetByIdAsync(id);
            if (existingPlan == null)
            {
                new MembershipPlanResponse
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

            _repo.Update(existingPlan);
            await _unitOfWork.SaveChangesAsync();
            return new MembershipPlanResponse
            {
                Success = true,
                Message = "Updated successfully"
            };
        }
    }
}
