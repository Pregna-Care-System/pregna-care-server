using Microsoft.AspNetCore.Mvc;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.MotherInfo
{
    [Route("")]
    [ApiController]
    public class MotherInfoController : ControllerBase
    {
        private readonly IMotherInfoService _motherInfoService;

        public MotherInfoController(IMotherInfoService motherInfoService)
        {
            _motherInfoService = motherInfoService;
        }

        [HttpGet("api/v1/User/{userId}/MotherInfo")]
        public IActionResult GetMotherInfosByUserId([FromRoute] Guid userId)
        {
            var responseList = _motherInfoService.GetAllMotherInfosByUserId(userId).Select(x => new
            {
                Id = x.Id,
                MotherName = x.MotherName,
                DateOfBirth = x.DateOfBirth,
                BloodType = x.BloodType,
                HealthStatus = x.HealthStatus,
                Notes = x.Notes,
            });

            if (responseList.Any())
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = responseList
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });
        }
    }
}
