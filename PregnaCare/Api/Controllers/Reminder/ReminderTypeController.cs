using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PregnaCare.Api.Controllers.Reminder
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReminderTypeController : ControllerBase
    {
        private readonly IReminderTypeService _reminderTypeService;

        public ReminderTypeController(IReminderTypeService reminderTypeService)
        {
            _reminderTypeService = reminderTypeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminderType([FromBody] ReminderTypeRequest request)
        {
            await _reminderTypeService.CreateReminderType(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reminderTypeService.GetAllReminderType();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReminderType(Guid id, [FromBody] ReminderTypeRequest request)
        {
            await _reminderTypeService.UpdateReminderType(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminderType(Guid id)
        {
            await _reminderTypeService.DeleteReminderType(id);
            return NoContent();
        }
    }
}
