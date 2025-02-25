using Microsoft.AspNetCore.Mvc;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Notification
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notiService;
        public NotificationController(INotificationService notiService)
        {
            _notiService = notiService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllNotificationByUserId(Guid userId)
        {
            var result = await _notiService.GetAllNotificationsByUserId(userId);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            await _notiService.DeleteNotification(id);
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNotificationIsRead(Guid id)
        {
            await _notiService.UpdateIsReadNotification(id);
            return NoContent();
        }
    }
}
