using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreatePaymentUrl(PaymentRequest request, HttpContext context);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
