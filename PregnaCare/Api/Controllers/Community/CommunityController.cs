using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Community
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _service;

        public CommunityController(ICommunityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllCommunities();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var community = await _service.GetCommunityById(id);
            if (community == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(community);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommunityRequest request)
        {
            var response = await _service.CreateCommunity(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CommunityRequest request)
        {
            var response = await _service.UpdateCommunity(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var success = await _service.DeleteCommunity(id);
            if (!success) return BadRequest(Messages.GetMessageById(Messages.E00013));
            return Ok(Messages.GetMessageById("Blog deleted successfully."));
        }
    }
}
