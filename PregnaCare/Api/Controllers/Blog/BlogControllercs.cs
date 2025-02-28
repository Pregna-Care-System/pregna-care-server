using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Blog
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _service;

        public BlogController(IBlogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var response = await _service.GetAllBlogs();
            return Ok(response);
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetAllBlogByUser(Guid id)
        {
            var response = await _service.GetAllBlogs();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var blog = await _service.GetBlogById(id);
            if (blog == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogRequest request, Guid tagId)
        {
            var response = await _service.CreateBlog(request, tagId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] BlogRequest request, Guid id)
        {
            var response = await _service.UpdateBlog(request, id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {   
            await _service.DeleteBlog(id);
            return NoContent();
        }
    }
}