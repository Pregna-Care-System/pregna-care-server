using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.PaymentResponseModel
{
    public class PaymentCallbackResponse : AbstractApiResponse<PaymentCallbackResult>
    {
        public override PaymentCallbackResult Response { get; set; }
    }


    public class PaymentCallbackResult
    {
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ResponseCode { get; set; }
    }
}
