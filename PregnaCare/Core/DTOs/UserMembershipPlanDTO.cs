namespace PregnaCare.Core.DTOs
{
    public class UserMembershipPlanDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public Guid MembershipPlanId { get; set; }

        public string MembershipPlanName { get; set; } = string.Empty;
        public DateTime? ActivatedAt { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal Price { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
