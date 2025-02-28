using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Blog>> GetAllActiveBlogAsync()
        {
            var blogList = await _context.Blogs.Where(b => b.IsDeleted == false).ToListAsync();
            return blogList;
        }

        public async Task<IEnumerable<Blog>> GetAllActiveBlogByUserIdAsync(Guid id)
        {
            var blogList = await _context.Blogs.Where(b => b.IsDeleted == false && b.UserId == id).ToListAsync();
            return blogList;
        }
    }
}
