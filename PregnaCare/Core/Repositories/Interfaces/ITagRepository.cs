using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag, Guid>
    {
        Task<IEnumerable<Tag>> GetAllActiveTagAsync();

    }
}
