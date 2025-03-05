using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.TagRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Tag
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var response = await _service.GetAllTags();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var tag = await _service.GetTagById(id);
            if (tag == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(TagRequest request)
        {
            var response = await _service.CreateTag(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] TagRequest request, Guid id)
        {
            var response = await _service.UpdateTag(request, id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _service.DeleteTag(id);
            return NoContent();
        }
    }
}