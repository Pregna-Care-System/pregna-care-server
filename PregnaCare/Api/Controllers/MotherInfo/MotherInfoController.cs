using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.MotherInfoModel;
using PregnaCare.Api.Models.Requests.MotherInfoRequestModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.MotherInfo
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MotherInfoController : ControllerBase
    {
        private readonly IMotherInfoService _motherInfoService;

        public MotherInfoController(IMotherInfoService motherInfoService)
        {
            _motherInfoService = motherInfoService;
        }

        [HttpGet("/api/v1/User/{userId}/MotherInfo")]
        public IActionResult GetMotherInfosByUserId([FromRoute] Guid userId)
        {
            var responseList = _motherInfoService.GetAllMotherInfosByUserId(userId).Select(x => new
            {
                Id = x.Id,
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

        [HttpPost]
        public async Task<IActionResult> CreateMotherInfo([FromBody] CreateMotherInfoRequest request)
        {
            var response = await _motherInfoService.CreateMotherInfoAsync(request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{motherInfoId}")]
        public async Task<IActionResult> UpdateMotherInfo([FromRoute] Guid motherInfoId, [FromBody] UpdateMotherInfoRequest request)
        {
            var response = await _motherInfoService.UpdateMotherInfoAsync(motherInfoId, request);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}
