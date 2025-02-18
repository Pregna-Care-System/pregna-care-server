using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Reminder
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] ReminderRequest request)
        {
            await _reminderService.CreateReminder(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reminderService.GetAllReminders();
            return Ok(result);
        }

        [HttpGet("Available")]
        public async Task<IActionResult> GetAllActiveReminder()
        {
            var result = await _reminderService.GetAllActiveReminders();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReminder(Guid id, [FromBody] ReminderRequest request)
        {
            await _reminderService.UpdateReminder(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            await _reminderService.DeleteReminder(id);
            return NoContent();
        }
    }
}
