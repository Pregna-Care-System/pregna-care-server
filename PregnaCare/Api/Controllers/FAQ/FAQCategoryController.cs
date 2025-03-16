using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.FAQCategoryRequestModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.FAQ
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FAQCategoryController : ControllerBase
    {
        private readonly IFAQCategoryService _faqCategoryService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fAQCategoryService"></param>
        public FAQCategoryController(IFAQCategoryService fAQCategoryService)
        {
            _faqCategoryService = fAQCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFAQCategories([FromQuery] string? name)
        {
            var response = await _faqCategoryService.GetCategoriesAsync(name);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFAQCategoryById([FromRoute] Guid id)
        {
            var response = await _faqCategoryService.GetCategoryByIdAsync(id);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFAQCategory([FromBody] CreateFAQCategoryRequest request)
        {
            var response = await _faqCategoryService.CreateCategoryAsync(request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFAQCategory([FromRoute] Guid id, [FromBody] UpdateFAQCategoryRequest request)
        {
            var response = await _faqCategoryService.UpdateCategoryAsync(id, request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQCategory([FromRoute] Guid id)
        {
            var response = await _faqCategoryService.DeleteCategoryAsync(id);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
