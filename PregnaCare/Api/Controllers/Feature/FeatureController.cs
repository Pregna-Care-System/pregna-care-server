using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
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

        [HttpDelete("Delete")]
        public async Task<FeatureResponse> DeleteFeature(Guid id)
        {
            var response = await _featureService.Delete(id);
            return response;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeaturesByUserId([FromQuery] Guid userId)
        {
            var responseList = await _featureService.GetAllFeaturesByUserIdAsync(userId);
            if (responseList != null && responseList.Any())
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = responseList
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });
        }

    }
}
