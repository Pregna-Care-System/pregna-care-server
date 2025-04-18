﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllActive([FromQuery] string? type = "Blog")
        {
            var response = await _service.GetAllBlogs(type);
            return Ok(response);
        }
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAllActiveAdmin([FromQuery] string? type = "Blog")
        {
            var response = await _service.GetAllBlogsAdmin(type);
            return Ok(response);
        }
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetAllBlogByUser(Guid id, [FromQuery] string? type = "Blog")
        {
            var response = await _service.GetAllByUserIdBlogs(id, type);
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
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequest request)
        {
            var response = await _service.CreateBlog(request, request.TagIds);
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

        [HttpPost("{id}/View")]
        public async Task<IActionResult> IncreaseViewCount([FromRoute] Guid id)
        {
            var response = await _service.IncreaseViewCount(id);
            if (!response) return BadRequest(Messages.GetMessageById(Messages.E00013));
            return Ok();
        }

        [HttpPut("{blogId}/Approve")]
        public async Task<IActionResult> IncreaseViewCount([FromRoute] Guid blogId, [FromQuery] string status)
        {
            var response = await _service.ApproveBlog(blogId, status);
            if (!response) return BadRequest(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013)
            });

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001)
            });
        }
    }
}