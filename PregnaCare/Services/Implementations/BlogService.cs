using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly List<BlogRequests> _blogs = new List<BlogRequests>();

        public async Task<IEnumerable<BlogResponse>> GetAllBlogs()
        {
            return await Task.FromResult(_blogs.Select(b => new BlogResponse { Response = b.Title }));
        }

        public async Task<BlogResponse> GetBlogById(Guid id)
        {
            var blog = _blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return null;
            return await Task.FromResult(new BlogResponse { Response = blog.Title });
        }

        public async Task<BlogResponse> CreateBlog(BlogRequests request)
        {
            request.Id = Guid.NewGuid();
            _blogs.Add(request);
            return await Task.FromResult(new BlogResponse { Success = true, Response = "Blog created successfully" });
        }

        public async Task<BlogResponse> UpdateBlog(BlogRequests request)
        {
            var blog = _blogs.FirstOrDefault(b => b.Id == request.Id);
            if (blog == null)
                return new BlogResponse { Success = false, Response = "Blog not found" };

            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.Author = request.Author;

            return await Task.FromResult(new BlogResponse { Success = true, Response = "Blog updated successfully" });
        }

        public async Task<bool> DeleteBlog(Guid id)
        {
            var blog = _blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return false;
            _ = _blogs.Remove(blog);
            return await Task.FromResult(true);
        }
    }
}
