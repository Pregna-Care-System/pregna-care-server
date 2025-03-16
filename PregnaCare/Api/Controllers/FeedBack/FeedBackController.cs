using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Requests.FeedBackRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.FeedBack
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var response = await _feedBackService.GetAllFeedbackAsync();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _feedBackService.GetFeedbackByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Create([FromBody] FeedbackRequest request, Guid userId)
        {
            var response = await _feedBackService.AddFeedbackAsync(request, userId);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _feedBackService.Delete(id);
            return NoContent();
        }
    }
}
