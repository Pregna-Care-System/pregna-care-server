namespace PregnaCare.Api.Models.Requests
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipPlanId { get; set; }
    }
}
