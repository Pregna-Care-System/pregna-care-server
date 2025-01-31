﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.MembershipPlan
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MembershipPlanController : AbstractApiController<MembershipPlanRequest, MembershipPlanResponse>
    {
        private readonly IMembershipPlansService _planService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="planService"></param>
        public MembershipPlanController(IMembershipPlansService planService)
        {
            _planService = planService;
        }

        [HttpPost("Create")]
        public override async Task<MembershipPlanResponse> Exec(MembershipPlanRequest request)
        {
            List<Guid> featureId = request.featuredId;

            var response = await _planService.AddPlanAsync(request, featureId);
            return response;
        }

        [HttpGet("GetAll")]
        public async Task<MembershipPlanListResponse> GetAllMembershipPlan()
        {
            var response = await _planService.GetAllPlansAsync();
            return response;
        }

        [HttpGet("GetById")]
        public async Task<MembershipPlanResponse> GetMembershipPlanById(Guid id)
        {
            var response = await _planService.GetPlanByIdAsync(id);
            return response;
        }

        [HttpDelete("Delete")]
        public async Task<MembershipPlanResponse> DeleteMembershipPlan(Guid id)
        {
            var response = await _planService.DeletePlanAsync(id);
            return response;
        }

        [HttpPut("Update")]
        public async Task<MembershipPlanResponse> UpdateMembershipPlan(Guid id, MembershipPlanRequest request)
        {
            var response = await _planService.UpdatePlanAsync(id, request);
            return response;
        }
    }
}
