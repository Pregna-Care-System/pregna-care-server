using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment, Guid>
    {
        Task<IEnumerable<Comment>> GetAllActiveCommentAsync(Guid blogId);
    }
}
