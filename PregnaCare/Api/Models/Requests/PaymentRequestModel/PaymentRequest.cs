namespace PregnaCare.Api.Models.Requests.PaymentRequestModel
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipPlanId { get; set; }
    }
}
