using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAll()
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
        public async Task<IActionResult> Create(BlogRequests request)
        {
            var response = await _service.CreateBlog(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(BlogRequests request)
        {
            var response = await _service.UpdateBlog(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var success = await _service.DeleteBlog(id);
            if (!success) return BadRequest(Messages.GetMessageById(Messages.E00013));
            return Ok("Blog deleted successfully.");
        }
    }
}