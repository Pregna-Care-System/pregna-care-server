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

        public PaymentController(IPaymentService service)
        {
            _service = service;
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
            if(response.Success) return Ok(response);

            return BadRequest(new
            {
                Success = false,
                Message = Messages.GetMessageById(Messages.E00000),
            });
        }
    }
}

