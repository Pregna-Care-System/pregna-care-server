using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.PaymentRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Payment
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        public PaymentController(IPaymentService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("Initiate")]
        public async Task<IActionResult> InitiatePayment(PaymentInitiationRequest request)
        {
            var result = await _service.InitiatePayment(request, HttpContext);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var result = (await _service.ProcessCallback(Request.Query)).Response;

            // Always redirect to frontend regardless of payment result
            // The frontend will display appropriate message based on status
            var frontendBaseUrl = _configuration["FrontEndBaseUrl"];
            var redirectUrl = $"{frontendBaseUrl}checkout/result?status={result.Status}&paymentId={result.PaymentId}&responseCode={result.ResponseCode}";

            return Redirect(redirectUrl);
        }


        [HttpPost]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequest request)
        {
            var url = _service.CreatePaymentUrl(request, HttpContext);
            if (string.IsNullOrEmpty(url)) return BadRequest(new
            {
                Success = false,
                MessageId = Messages.E00000,
                Message = Messages.GetMessageById(Messages.E00000),
            });

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Url = url,
            });
        }

        [HttpGet]
        public IActionResult CallBack()
        {
            var response = _service.PaymentExecute(Request.Query);
            var frontendBaseUrl = _configuration["FrontEndBaseUrl"];
            if (response.Success)
            {
                return Redirect(frontendBaseUrl + $"checkout/result?vnp_ResponseCode=00");
            }

            return Redirect(frontendBaseUrl + $"checkout/result?vnp_ResponseCode={Request.Query["vnp_ResponseCode"]}");
        }
    }
}

