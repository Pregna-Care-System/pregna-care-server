using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IFeatureRepository : IGenericRepository<Feature, Guid>
    {
        Task<IEnumerable<Feature>> GetActiveFeatureAsync();
        Task<IEnumerable<Feature>> GetAllFeaturesByUserId(Guid userId);
    }
}
