using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;

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
