using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class TagRepository : GenericRepository<Tag, Guid>, ITagRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public TagRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<Tag>> GetAllActiveTagAsync()
        {
            var blogList = await _appDbContext.Tags.Where(b => b.IsDeleted == false).ToListAsync();
            return blogList;
        }
    }
}
