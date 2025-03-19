using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.ReactionRequestModel;
using PregnaCare.Services.BackgroundServices;

namespace PregnaCare.Api.Controllers.Reaction
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reactionService"></param>
        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpGet("/api/v1/Blog/{blogId}/[controller]")]
        public async Task<IActionResult> SelectReactionByBlogId([FromRoute] Guid blogId)
        {
            var response = await _reactionService.SelectReactionByBlogIdAsync(blogId);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("/api/v1/Comment/{commentId}/[controller]")]
        public async Task<IActionResult> SelectReactionByCommentId([FromRoute] Guid commentId)
        {
            var response = await _reactionService.SelectReactionByCommentIdAsync(commentId);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction([FromBody] CreateReactionRequest request)
        {
            var response = await _reactionService.CreateReactionAsync(request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReaction([FromRoute] Guid id, [FromBody] UpdateReactionRequest request)
        {
            var response = await _reactionService.UpdateReactionAsync(id, request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReaction([FromRoute] Guid id)
        {
            var response = await _reactionService.DeleteReactionAsync(id);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
