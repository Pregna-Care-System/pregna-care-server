using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IFeedBackRepository : IGenericRepository<FeedBack, Guid>
    {
        Task<IEnumerable<FeedBackDTO>> GetActiveFeatureAsync();
        Task<FeedBackDTO> GetFeedBackByIdAsync(Guid id);
    }
}
