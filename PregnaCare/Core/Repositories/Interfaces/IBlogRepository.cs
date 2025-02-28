using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IBlogRepository: IGenericRepository<Blog, Guid>
    {
        Task<IEnumerable<Blog>> GetAllActiveBlogAsync();
        Task<IEnumerable<Blog>> GetAllActiveBlogByUserIdAsync(Guid id);

    }
}
