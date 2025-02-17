using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces

{
    public interface IBlogService
    {
        Task<IEnumerable<BlogResponse>> GetAllBlogs();
        Task<BlogResponse> GetBlogById(Guid id);
        Task<BlogResponse> CreateBlog(BlogRequests request);
        Task<BlogResponse> UpdateBlog(BlogRequests request);
        Task<bool> DeleteBlog(Guid id);
    }
}
