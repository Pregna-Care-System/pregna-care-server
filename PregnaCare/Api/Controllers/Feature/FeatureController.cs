using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Feature
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeatureController : AbstractApiController<FeatureRequest, FeatureResponse>
    {
        private readonly IFeatureService _featureService;
        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }
        [HttpPost("Create")]
        public override async Task<FeatureResponse> Exec(FeatureRequest request)
        {
            var response = await _featureService.AddFeatureAsync(request);
            return response;
        }

        [HttpGet("GetAll")]
        public async Task<FeatureListResponse> GetAllListFeature()
        {
            var response = await _featureService.GetAllFeaturesAsync();
            return response;
        }

        [HttpGet("GetById")]
        public async Task<FeatureResponse> GetFeatureById(Guid id)
        {
            var response = await _featureService.GetFeatureById(id);
            return response;
        }

        [HttpPut("Update")]
        public async Task<FeatureResponse> UpdateFeature(Guid id, FeatureRequest request) 
        {
            var response = await _featureService.Update(id, request);
            return response;

        }

    }
}
