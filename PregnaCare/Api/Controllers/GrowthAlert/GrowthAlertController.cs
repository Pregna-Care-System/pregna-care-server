using Microsoft.AspNetCore.Mvc;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.GrowthAlert
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GrowthAlertController : ControllerBase
    {
        private readonly IGrowthAlertService _growthAlertService;

        public GrowthAlertController(IGrowthAlertService growthAlertService)
        {
            _growthAlertService = growthAlertService;
        }

        [HttpGet("api/v1/User/{userId}/GrowthAlert")]
        public async Task<IActionResult> GetGrowthAlerts([FromRoute] Guid userId)
        {
            var growthAlerts = await _growthAlertService.GetGrowthAlerts(userId);

            if (growthAlerts.Any())
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = growthAlerts.Select(x => new
                    {
                        x.Id,
                        x.FetalGrowthRecordId,
                        x.Week,
                        x.AlertDate,
                        x.AlertFor,
                        x.Issue,
                        x.Severity,
                        x.Recommendation,
                        x.IsResolved,
                        x.Status,
                    })
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromQuery] string status)
        {
            var result = await _growthAlertService.UpdateStatusGrowthAlert(id, status);
            if (result)
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00000,
                Message = Messages.GetMessageById(Messages.E00000),
            });
        }
    }
}
