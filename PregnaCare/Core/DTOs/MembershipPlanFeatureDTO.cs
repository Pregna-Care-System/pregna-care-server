namespace PregnaCare.Core.DTOs
{
    public class MembershipPlanFeatureDTO
    {
        public Guid MembershipPlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public double Price { get; set; } 
        public int Duration { get; set; } 
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public List<FeatureDTO> Features { get; set; }
    }
}
