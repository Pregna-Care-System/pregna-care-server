using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.CommentBlogRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Comment
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet("Blog/{blogId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid blogId)
        {
            var response = await _service.GetAllBlogComment(blogId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var comment = await _service.GetCommentById(id);
            if (comment == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            var response = await _service.CreateComment(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateCommentRequest request, Guid id)
        {
            var response = await _service.UpdateComment(request, id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _service.DeleteComment(id);
            return NoContent();
        }
    }
}
