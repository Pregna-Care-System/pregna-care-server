using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<User, Guid>
    {
        Task<IEnumerable<AccountDTO>> GetMembers(string filterType = null, string name = null);
        Task<AccountDTO> GetMemberInforWithPlanDetail(Guid userId);

    }
}
