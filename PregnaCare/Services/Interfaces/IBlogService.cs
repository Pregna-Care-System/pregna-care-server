using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;

namespace PregnaCare.Services.Interfaces

{
    public interface IBlogService
    {
        Task<BlogListResponse> GetAllBlogs();
        Task<BlogListResponse> GetAllByUserIdBlogs(Guid id);
        Task<BlogResponse> GetBlogById(Guid id);
        Task<BlogResponse> CreateBlog(BlogRequest request, Guid tagId);
        Task<BlogResponse> UpdateBlog(BlogRequest request, Guid id);
        Task DeleteBlog(Guid id);
    }
}
