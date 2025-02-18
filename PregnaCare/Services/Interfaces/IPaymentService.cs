﻿using PregnaCare.Api.Models.Requests.PaymentRequestModel;
using PregnaCare.Api.Models.Responses.PaymentResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreatePaymentUrl(PaymentRequest request, HttpContext context);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
