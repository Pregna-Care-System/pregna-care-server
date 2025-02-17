using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IFeatureRepository : IGenericRepository<Feature, Guid>
    {
        Task<IEnumerable<Feature>> GetActiveFeatureAsync();
    }
}
