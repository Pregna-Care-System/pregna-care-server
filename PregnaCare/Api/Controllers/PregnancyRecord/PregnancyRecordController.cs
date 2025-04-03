using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.PregnancyRecordRequestModel;
using PregnaCare.Api.Models.Responses.PregnancyRecordResponseModel;
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

        [HttpGet("/api/v1/User/{motherInfoId}/[controller]")]
        public async Task<IActionResult> GetAllPregancyRecordsByMotherInfoId([FromRoute] Guid motherInfoId)
        {
            var response = (await _service.GetAllPregnancyRecords(motherInfoId)).Select(x => new SelectPregnancyRecordResponse
            {
                Id = x.Id,
                BabyName = x.BabyName,
                BabyGender = x.BabyGender,
                PregnancyStartDate = x.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now),
                ExpectedDueDate = x.ExpectedDueDate ?? DateOnly.FromDateTime(DateTime.Now),
                ImageUrl = x.ImageUrl,
                TotalWeeks = (x.ExpectedDueDate.Value.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber) / 7,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                GestationalAgeResponse = _service.CalculateGestationalAge(x.PregnancyStartDate.Value.ToDateTime(TimeOnly.MinValue)),
            });

            if (response.Any()) return Ok(new
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

        [HttpGet("{pregnancyRecordId}")]
        public async Task<IActionResult> GetPregnancyRecordById([FromRoute] Guid pregnancyRecordId)
        {
            var entity = (await _service.GetPregnancyRecordById(pregnancyRecordId));
            var response = new SelectPregnancyRecordResponse
            {
                Id = entity.Id,
                BabyName = entity.BabyName,
                BabyGender = entity.BabyGender,
                PregnancyStartDate = entity.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now),
                ExpectedDueDate = entity.ExpectedDueDate ?? DateOnly.FromDateTime(DateTime.Now),
                ImageUrl = entity.ImageUrl,
                TotalWeeks = (entity.ExpectedDueDate.Value.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber) / 7,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                GestationalAgeResponse = _service.CalculateGestationalAge(entity.PregnancyStartDate.Value.ToDateTime(TimeOnly.MinValue)),
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

        [HttpPut("{pregnancyRecordId}")]
        public async Task<IActionResult> UpdatePregnancyRecord([FromRoute] Guid pregnancyRecordId, [FromBody] UpdatePregnancyRecordRequest request)
        {
            var response = await _service.UpdatePregnancyRecord(pregnancyRecordId, request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("/api/v1/[controller]/{pregnancyRecordId}")]
        public async Task<IActionResult> DeletePregnancyRecord([FromRoute] Guid pregnancyRecordId)
        {
            var response = await _service.DeletePregnancyRecord(pregnancyRecordId);

            if (response) return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001)
            });

            return BadRequest(new
            {
                Success = false,
                MessageId = Messages.E00000,
                Message = Messages.GetMessageById(Messages.E00000)
            });
        }
    }
}
