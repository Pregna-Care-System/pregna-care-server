using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.FAQRequestModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.FAQ
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="faqService"></param>
        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateFAQRequest request)
        {
            var response = await _faqService.CreateFAQAsync(request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFAQ([FromRoute] Guid id, [FromBody] UpdateFAQRequest request)
        {
            var response = await _faqService.UpdateFAQAsync(id, request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ([FromRoute] Guid id)
        {
            var response = await _faqService.DeleteFAQAsync(id);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
