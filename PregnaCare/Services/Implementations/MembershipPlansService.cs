﻿using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class MembershipPlansService : IMembershipPlansService
    {
        private readonly IMembershipPlansRepository _repo;

        public MembershipPlansService (IMembershipPlansRepository membershipPlansRepository)
        {
            _repo = membershipPlansRepository;
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

        public async Task DeletePlanAsync(Guid id)
        {
            var plan = await _repo.GetByIdAsync(id);
            if(plan != null)
            {
                plan.IsDeleted = true;
                _repo.Update(plan);
            }
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

        public async Task<MembershipPlan> GetPlanByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task UpdatePlanAsync(Guid id, MembershipPlan plan)
        {
            var existingPlan = await _repo.GetByIdAsync(id);
            if (existingPlan != null)
            {
                existingPlan.PlanName = plan.PlanName;
                existingPlan.Price = plan.Price;
                existingPlan.Description = plan.Description;
                existingPlan.Duration = plan.Duration;
                
                _repo.Update(existingPlan);
            }
        }
    }
}
