using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IUserMembershipPlanRepository : IGenericRepository<UserMembershipPlan, Guid>
    {
        Task<IEnumerable<UserMembershipPlanDTO>> GetUserMembershipPlanList();
    }
}
