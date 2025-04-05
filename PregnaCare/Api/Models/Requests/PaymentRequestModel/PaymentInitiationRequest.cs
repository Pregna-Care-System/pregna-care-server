namespace PregnaCare.Api.Models.Requests.PaymentRequestModel
{
    public class PaymentInitiationRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipPlanId { get; set; }
        public string UserEmail { get; set; }
    }
}
