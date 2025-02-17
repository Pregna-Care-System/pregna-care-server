using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IMembershipPlansRepository : IGenericRepository<MembershipPlan, Guid>
    {
        Task<IEnumerable<MembershipPlanFeatureDTO>> GetPlansWithFeaturesAsync();

        Task AddPlanAsync(MembershipPlan plan, List<Guid> featureIds);
        Task<MembershipPlanFeatureDTO> GetPlanByName(string name);
        Task<MembershipPlanFeatureDTO> GetPlanById(Guid id);

        Task DeletePlanAsync(Guid planId);
        Task Update(MembershipPlan plan, List<Guid> featureIds);
    }
}
