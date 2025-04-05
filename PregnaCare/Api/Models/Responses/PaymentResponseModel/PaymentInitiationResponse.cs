using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.PaymentResponseModel
{
    public class PaymentInitiationResponse : AbstractApiResponse<PaymentInitiationResult>
    {
        public override PaymentInitiationResult Response { get; set; }
    }

    public class PaymentInitiationResult
    {
        public string PaymentUrl { get; set; }
        public Guid PaymentId { get; set; }
    }
}
