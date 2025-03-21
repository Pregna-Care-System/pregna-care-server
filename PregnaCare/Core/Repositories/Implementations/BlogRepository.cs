using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Utils;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class BlogRepository : GenericRepository<Blog, Guid>, IBlogRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public BlogRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<BlogDTO>> GetAllActiveBlogAsync(string type)
        {
            var blogList = await _context.Blogs
                .Where(b => b.IsDeleted == false && b.IsVisible == true && b.Type.ToLower() == type.ToLower()).Include(x => x.User)
                .OrderByDescending(b => b.UpdatedAt)
                .ThenByDescending(b => b.CreatedAt)
                .Select(blog => new BlogDTO
                {
                    Id = blog.Id,
                    UserId = blog.UserId,
                    FullName = blog.User.FullName,
                    PageTitle = blog.PageTitle,
                    Heading = blog.Heading,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    IsVisible = blog.IsVisible,
                    SharedChartData = blog.SharedChartData,
                    Status = blog.Status,
                    Type = blog.Type,
                    ViewCount = blog.ViewCount ?? 0,
                    TimeAgo = CommonUtils.GetTimeAgo(blog.UpdatedAt.Value),
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

        public async Task<IEnumerable<BlogDTO>> GetAllActiveBlogByUserIdAsync(Guid userId, string type)
        {
            var blogList = await _context.Blogs
                .Where(b => b.IsDeleted == false && b.UserId == userId && b.Type.ToLower() == type.ToLower())
                .OrderByDescending(b => b.UpdatedAt)
                .ThenByDescending(b => b.CreatedAt)
                .Include(x => x.User)
                .Select(blog => new BlogDTO
                {
                    Id = blog.Id,
                    UserId = blog.UserId,
                    FullName = blog.User.FullName,
                    PageTitle = blog.PageTitle,
                    Heading = blog.Heading,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    IsVisible = blog.IsVisible,
                    SharedChartData = blog.SharedChartData,
                    Status = blog.Status,
                    Type = blog.Type,
                    TimeAgo = CommonUtils.GetTimeAgo(blog.UpdatedAt.Value),
                    ViewCount = blog.ViewCount ?? 0,
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
