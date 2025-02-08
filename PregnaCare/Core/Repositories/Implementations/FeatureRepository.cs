using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class FeatureRepository : GenericRepository<Feature, Guid>, IFeatureRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public FeatureRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<Feature>> GetActiveFeatureAsync()
        {
            return await _appDbContext.Features.Where(f => (bool)!f.IsDeleted).ToListAsync();
        }
    }
}
