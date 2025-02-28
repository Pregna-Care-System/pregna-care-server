using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IBlogRepository blogRepository, IUnitOfWork unitOfWork)
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BlogListResponse> GetAllBlogs()
        {
            var blogs = await _blogRepository.GetAllActiveBlogAsync();
            return new BlogListResponse
            {
                Success = true,
                Response = blogs
            };
        }

        public async Task<BlogResponse> GetBlogById(Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            return new BlogResponse
            {
                Success = true,
                Response = blog
            };
        }

        public async Task<BlogResponse> CreateBlog(BlogRequest request, Guid tagId)
        {
            var response = new BlogResponse();

            var blog = Mapper.MapToBlog(request);
            blog.Id = Guid.NewGuid();
            blog.CreatedAt = DateTime.Now;
            blog.UpdatedAt = DateTime.Now;
            blog.IsDeleted = false;

            await _blogRepository.AddAsync(blog);

            var blogTag = new BlogTag
            {
                Id = Guid.NewGuid(),
                BlogId = blog.Id,
                TagId = tagId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };
            await _unitOfWork.GetRepository<BlogTag, Guid>().AddAsync(blogTag);
            await _unitOfWork.SaveChangesAsync();

            response.Response = blog;
            response.Success = true;
            response.Message = "Create new blog successfully";
            return response;
        }

        public async Task<BlogResponse> UpdateBlog(BlogRequest request, Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);

            blog.ShortDescription = request.ShortDescription;
            blog.Content = request.Content;
            blog.FeaturedImageUrl = request.FeaturedImageUrl;
            blog.Heading = request.Heading;
            blog.PageTitle = request.PageTitle;
            blog.UpdatedAt = DateTime.Now;

             _blogRepository.Update(blog);
            await _unitOfWork.SaveChangesAsync();
            return new BlogResponse
            {
                Success = true,
                Response = blog
            };
        }

        public async Task DeleteBlog(Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            _blogRepository.Remove(blog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BlogListResponse> GetAllByUserIdBlogs(Guid id)
        {
            var blogs = await _blogRepository.GetAllActiveBlogByUserIdAsync(id);
            return new BlogListResponse
            {
                Success = true,
                Response = blogs
            };
        }
    }
}
