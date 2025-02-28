using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class CommentRepository : GenericRepository<Comment, Guid>, ICommentRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public CommentRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<Comment>> GetAllActiveCommentAsync(Guid blogId)
        {
            var commentList = await _appDbContext.Comments
                .Where(b => b.IsDeleted == false && b.BlogId == blogId)
                .ToListAsync();
            return commentList;
        }
    }
}
