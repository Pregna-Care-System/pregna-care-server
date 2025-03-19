using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.ContactSubscriberRequestModel;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.ContactUs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contactService"></param>
        /// <param name="emailService"></param>
        public ContactController(IContactService contactService, IEmailService emailService)
        {
            _contactService = contactService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactAsync([FromBody] CreateContactRequest request)
        {
            var response = await _contactService.CreateContactAsync(request);
            if (!response.Success) return BadRequest(response);

            var unsubscribeUrl = Url.Action("Unsubscribe", "Contact", new { email = request.Email }, HttpContext.Request.Scheme);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "EventSubscriptionConfirmation.html");
            var emailContent = await System.IO.File.ReadAllTextAsync(path);

            emailContent = emailContent
                            .Replace("{FullName}", request.FullName)
                            .Replace("{Email}", request.Email)
                            .Replace("{SubscriptionDate}", DateTime.Now.ToString("MMMM d, yyyy"))
                            .Replace("{UnsubscribeUrl}", unsubscribeUrl);

            _ = _emailService.SendEmail(request.Email, "Thank You for Subscribing to PregnaCare Events", emailContent, "");
            return Ok(response);
        }

        [HttpGet("Unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromQuery] string email)
        {
            _ = await _contactService.DeleteContactAsync(email);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "Html", "UnsubscribeConfirmation.html");
            return PhysicalFile(filePath, "text/html");
        }

    }
}
