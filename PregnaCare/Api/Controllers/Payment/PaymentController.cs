using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
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

        [HttpPost]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequest request)
        {
            var url = _service.CreatePaymentUrl(request, HttpContext);
            if (string.IsNullOrEmpty(url)) return BadRequest(new
            {
                Success = false,
                Message = Messages.GetMessageById(Messages.E00000),
            });

            return Ok(new
            {
                Success = true,
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

