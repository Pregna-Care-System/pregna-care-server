using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class MembershipPlanRequest : AbstractApiRequest
    {
        public string PlanName { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Duration { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<Guid> featuredId { get; set; }

    }
}
