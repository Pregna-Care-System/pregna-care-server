﻿using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IMembershipPlansService
    {
        Task<MembershipPlanListResponse> GetAllPlansAsync();
        Task<MembershipPlanResponse> GetPlanByIdAsync(Guid id);
        Task<MembershipPlanResponse> AddPlanAsync(MembershipPlanRequest request, List<Guid> featureIds);
        Task<MembershipPlanResponse> UpdatePlanAsync(Guid id, MembershipPlanRequest plan);
        Task<MembershipPlanResponse> DeletePlanAsync(Guid id);

    }
}
