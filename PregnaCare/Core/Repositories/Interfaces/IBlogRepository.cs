using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog, Guid>
    {
        Task<IEnumerable<BlogDTO>> GetAllActiveBlogAsync(string type = "Blog");
        Task<IEnumerable<BlogDTO>> GetAllActiveBlogAdminAsync(string type = "Blog");

        Task<IEnumerable<BlogDTO>> GetAllActiveBlogByUserIdAsync(Guid id, string type = "Blog");
        Task<BlogDTO> GetDetailById(Guid id);

    }
}
