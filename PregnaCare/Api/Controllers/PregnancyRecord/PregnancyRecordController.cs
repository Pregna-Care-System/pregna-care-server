using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.PregnancyRecord
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class PregnancyRecordController : ControllerBase
    {
        private readonly IPregnancyRecordService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public PregnancyRecordController(IPregnancyRecordService service)
        {
            _service = service;
        }

        [HttpGet("/api/v1/User/{userId}/[controller]")]
        public async Task<IActionResult> GetAllPregancyRecordsByUserId([FromRoute] Guid userId)
        {
            var response = (await _service.GetAllPregnancyRecords(userId)).Select(x => new SelectPregnancyRecordResponse
            {
                Id = x.Id,
                MotherName = x.MotherInfo.MotherName,
                MotherDateOfBirth = x.MotherInfo?.DateOfBirth ?? DateOnly.FromDateTime(DateTime.Now),
                BloodType = x.MotherInfo?.BloodType ?? string.Empty,
                HealhStatus = x.MotherInfo?.HealthStatus ?? string.Empty,
                Notes = x.MotherInfo?.Notes ?? string.Empty,
                BabyName = x.BabyName,
                BabyGender = x.BabyGender,
                PregnancyStartDate = x.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now),
                ExpectedDueDate = x.ExpectedDueDate ?? DateOnly.FromDateTime(DateTime.Now),
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            });

            if (response.Any()) return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = response
            });

            return NotFound(new {
                Succeess = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013)
            });
        }

        [HttpGet("/api/v1/User/{userId}/[controller]/{pregnancyRecordId}")]
        public async Task<IActionResult> GetPregnancyRecordById([FromRoute] Guid userId, [FromRoute] Guid pregnancyRecordId)
        {
            var entity = (await _service.GetPregnancyRecordById(userId, pregnancyRecordId));
            var response = new SelectPregnancyRecordResponse
            {
                Id = entity.Id,
                BabyName = entity.BabyName,
                BabyGender = entity.BabyGender,
                PregnancyStartDate = entity.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now),
                ExpectedDueDate = entity.ExpectedDueDate ?? DateOnly.FromDateTime(DateTime.Now),
                ImageUrl = entity.ImageUrl
            };

            if (response != null) return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = response
            });

            return NotFound(new
            {
                Succeess = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePregnancyRecord([FromBody] CreatePregnancyRecordRequest request)
        {
            var response = await _service.CreatePregnancyRecord(request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePregnancyRecord([FromBody] UpdatePregnancyRecordRequest request)
        {
            var response = await _service.UpdatePregnancyRecord(request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("/api/v1/[controller]/{pregnancyRecordId}")]
        public async Task<IActionResult> DeletePregnancyRecord([FromRoute] Guid pregnancyRecordId)
        {
            var response = await _service.DeletePregnancyRecord(pregnancyRecordId);

            if (response) return Ok(new
            {
                Succeess = false,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001)
            });

            return BadRequest(new
            {
                Succeess = false,
                MessageId = Messages.E00000,
                Message = Messages.GetMessageById(Messages.E00000)
            });
        }
    }
}
