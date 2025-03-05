using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog, Guid>
    {
        Task<IEnumerable<BlogDTO>> GetAllActiveBlogAsync();
        Task<IEnumerable<BlogDTO>> GetAllActiveBlogByUserIdAsync(Guid id);

    }
}
