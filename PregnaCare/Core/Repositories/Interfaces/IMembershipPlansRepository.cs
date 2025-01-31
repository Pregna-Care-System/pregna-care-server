using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IMembershipPlansRepository: IGenericRepository<MembershipPlan, Guid>
    {
        Task<IEnumerable<MembershipPlan>> GetActivePlanAsync();
        Task AddPlanAsync(MembershipPlan plan, List<Guid> featureIds);
    }
}
