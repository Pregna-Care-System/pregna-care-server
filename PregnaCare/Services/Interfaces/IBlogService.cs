using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;

namespace PregnaCare.Services.Interfaces

{
    public interface IBlogService
    {
        Task<BlogListResponse> GetAllBlogs(string type = "Blog");
        Task<BlogListResponse> GetAllBlogsAdmin(string type = "Blog");

        Task<BlogListResponse> GetAllByUserIdBlogs(Guid id, string type = "Blog");
        Task<SelectDetailBlogResponse> GetBlogById(Guid id);
        Task<BlogResponse> CreateBlog(BlogRequest request, List<Guid> tagIds);
        Task<BlogResponse> UpdateBlog(BlogRequest request, Guid id);
        Task DeleteBlog(Guid id);
        Task<bool> IncreaseViewCount(Guid blogId);
        Task<bool> ApproveBlog(Guid blogId, string status);
    }
}
