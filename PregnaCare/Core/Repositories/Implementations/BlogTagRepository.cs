using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class BlogTagRepository : GenericRepository<BlogTag, Guid>, IBlogTagRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public BlogTagRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }
    }
}
