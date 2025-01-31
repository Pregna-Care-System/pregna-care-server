using Microsoft.AspNetCore.Http;
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
    }
}
