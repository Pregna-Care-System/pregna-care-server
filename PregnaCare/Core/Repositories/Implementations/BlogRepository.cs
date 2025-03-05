using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class BlogRepository : GenericRepository<Blog, Guid>, IBlogRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public BlogRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<BlogDTO>> GetAllActiveBlogAsync()
        {
            var blogList = await _context.Blogs.Where(b => b.IsDeleted == false && b.IsVisible == true)
                .Select(blog => new BlogDTO
                {
                    id = blog.Id,
                    UserId = blog.UserId,
                    PageTitle = blog.PageTitle,
                    Heading = blog.Heading,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    IsVisible = blog.IsVisible,

                    Tags = _context.BlogTags
                                .Where(bt => bt.BlogId == blog.Id && bt.IsDeleted == false)
                                .Join(_context.Tags, bt => bt.TagId, t => t.Id, (bt, t) => new TagDTO
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                })
                                .ToList()
                })
                .ToListAsync();
            return blogList;
        }

        public async Task<IEnumerable<BlogDTO>> GetAllActiveBlogByUserIdAsync(Guid userId)
        {
            var blogList = await _context.Blogs
                .Where(b => b.IsDeleted == false && b.UserId == userId)
                .Select(blog => new BlogDTO
                {
                    id = blog.Id,
                    UserId = blog.UserId,
                    PageTitle = blog.PageTitle,
                    Heading = blog.Heading,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    IsVisible = blog.IsVisible,

                    Tags = _context.BlogTags
                                .Where(bt => bt.BlogId == blog.Id && bt.IsDeleted == false)
                                .Join(_context.Tags, bt => bt.TagId, t => t.Id, (bt, t) => new TagDTO
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                })
                                .ToList()
                })
                .ToListAsync();

            return blogList;
        }

    }
}
