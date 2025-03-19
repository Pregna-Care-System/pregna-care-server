using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class FeedBackRepository : GenericRepository<FeedBack, Guid>, IFeedBackRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;

        public FeedBackRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<FeedBackDTO>> GetActiveFeatureAsync()
        {
            return await _appDbContext.FeedBacks.Where(f => f.IsDeleted == false)
                .Select(f => new FeedBackDTO
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    email = f.User.Email,
                    Rating = f.Rating,
                    Content = f.Content,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted
                })
                .ToListAsync();
        }

        public async Task<FeedBackDTO> GetFeedBackByIdAsync(Guid id)
        {
            return await _appDbContext.FeedBacks.Where(f => f.IsDeleted == false && f.Id == id)
                .Select(f => new FeedBackDTO
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    email = f.User.Email,
                    Rating = f.Rating,
                    Content = f.Content,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    IsDeleted = f.IsDeleted
                })
                .FirstOrDefaultAsync();
        }
    }
}
