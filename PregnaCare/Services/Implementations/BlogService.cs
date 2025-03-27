using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
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
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IGenericRepository<Reaction, Guid> _reactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IBlogRepository blogRepository, IUnitOfWork unitOfWork, IBlogTagRepository blogTagRepository, ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _blogTagRepository = blogTagRepository;
            _commentRepository = commentRepository;
            _reactionRepository = unitOfWork.GetRepository<Reaction, Guid>();

        }

        public async Task<BlogListResponse> GetAllBlogs(string type)
        {
            var blogs = await _blogRepository.GetAllActiveBlogAsync(type);
            return new BlogListResponse
            {
                Success = true,
                Response = blogs
            };
        }
        public async Task<BlogListResponse> GetAllBlogsAdmin(string type)
        {
            var blogs = await _blogRepository.GetAllActiveBlogAdminAsync(type);
            return new BlogListResponse
            {
                Success = true,
                Response = blogs
            };
        }
        public async Task<SelectDetailBlogResponse> GetBlogById(Guid id)
        {
            var response = new SelectDetailBlogResponse() { Success = false };
            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = await _blogRepository.GetDetailById(id);
            return response;
        }

        public async Task<BlogResponse> CreateBlog(BlogRequest request, List<Guid> tagIds)
        {
            var response = new BlogResponse();

            var blog = Mapper.MapToBlog(request);
            blog.Id = Guid.NewGuid();
            blog.Type = request.Type;
            blog.SharedChartData = request.SharedChartData;
            blog.CreatedAt = DateTime.Now;
            blog.UpdatedAt = DateTime.Now;
            blog.IsDeleted = false;

            if(blog.Type.ToLower() == BlogTypeEnum.Community.ToString().ToLower())
            {
                blog.Status = StatusEnum.Approved.ToString();
            }
            else
            {
                blog.Status = StatusEnum.Pending.ToString();
            }

            await _unitOfWork.GetRepository<Blog, Guid>().AddAsync(blog);

            var blogTagRepo = _unitOfWork.GetRepository<BlogTag, Guid>();
            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    var blogTag = new BlogTag
                    {
                        Id = Guid.NewGuid(),
                        BlogId = blog.Id,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false
                    };

                    await blogTagRepo.AddAsync(blogTag);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            response.Response = blog;
            response.Success = true;
            response.Message = "Create new blog successfully";
            return response;
        }

        public async Task<BlogResponse> UpdateBlog(BlogRequest request, Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return new BlogResponse
                {
                    Success = false,
                    Message = "Blog does not found",
                };
            }
            blog.ShortDescription = request.ShortDescription;
            blog.Content = request.Content;
            blog.FeaturedImageUrl = request.FeaturedImageUrl;
            blog.Heading = request.Heading;
            blog.PageTitle = request.PageTitle;
            blog.UpdatedAt = DateTime.Now;
            blog.Type = request.Type;
            blog.SharedChartData = request.SharedChartData;
            blog.IsVisible = request.IsVisible;

            _unitOfWork.GetRepository<Blog, Guid>().Update(blog);

            if (request.TagIds != null && request.TagIds.Any())
            {

                var existingTags = await _blogTagRepository.FindAsync(x => x.BlogId == id);
                var newTagIds = request.TagIds ?? new List<Guid>();
                //remove blog tags
                foreach (var extingTag in existingTags)
                {
                    if (!newTagIds.Contains(extingTag.TagId))
                    {
                        _unitOfWork.GetRepository<BlogTag, Guid>().Remove(extingTag);
                    }
                }
                //add new blog tags
                foreach (var newTagId in newTagIds)
                {
                    if (!existingTags.Any(x => x.TagId == newTagId))
                    {
                        var blogTag = new BlogTag
                        {
                            Id = Guid.NewGuid(),
                            BlogId = blog.Id,
                            TagId = newTagId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsDeleted = false
                        };
                        await _unitOfWork.GetRepository<BlogTag, Guid>().AddAsync(blogTag);
                    }
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return new BlogResponse
            {
                Success = true,
                Message = "Update Blog Successfully",
                Response = blog
            };
        }

        public async Task DeleteBlog(Guid id)
        {
            var blogTags = await _blogTagRepository.GetAllAsync();
            var commentPost = await _commentRepository.GetAllAsync();
            var reactionPost = await _reactionRepository.GetAllAsync();

            var blog = await _blogRepository.GetByIdAsync(id);
            _ = _unitOfWork.GetRepository<BlogTag, Guid>();

            foreach (var blogTag in blogTags)
            {
                if (blogTag.BlogId == id)
                {
                    _unitOfWork.GetRepository<BlogTag, Guid>().Remove(blogTag);
                }
            }
            foreach (var comment in commentPost)
            {
                if (comment.BlogId == id)
                {
                    _unitOfWork.GetRepository<Comment, Guid>().Remove(comment);
                }
            }
            foreach (var reaction in reactionPost)
            {
                if (reaction.BlogId == id)
                {
                    _unitOfWork.GetRepository<Reaction, Guid>().Remove(reaction);
                }
            }

            _unitOfWork.GetRepository<Blog, Guid>().Remove(blog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BlogListResponse> GetAllByUserIdBlogs(Guid id, string type)
        {
            var blogs = await _blogRepository.GetAllActiveBlogByUserIdAsync(id, type);
            return new BlogListResponse
            {
                Success = true,
                Response = blogs
            };
        }

        public async Task<bool> IncreaseViewCount(Guid blogId)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if (blog == null) return false;

            blog.ViewCount++;
            _unitOfWork.GetRepository<Blog, Guid>().Update(blog);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveBlog(Guid blogId, string status)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if (blog == null) return false;

            var matchedStatus = Enum.GetValues<StatusEnum>()
                             .FirstOrDefault(s => s.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));

            if (!Enum.IsDefined(typeof(StatusEnum), matchedStatus))
            {
                return false;
            }

            blog.Status = matchedStatus.ToString();
            blog.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Blog, Guid>().Update(blog);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
