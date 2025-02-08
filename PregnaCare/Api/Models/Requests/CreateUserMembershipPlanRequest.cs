namespace PregnaCare.Api.Models.Requests
{
    public class CreateUserMembershipPlanRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
