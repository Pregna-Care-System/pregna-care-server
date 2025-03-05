using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.FetalGrowthRecordRequestModel;
using PregnaCare.Api.Models.Responses.FetalGrowthRecordResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.FetalGrowthRecord
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FetalGrowthRecordController : ControllerBase
    {
        private readonly IFetalGrowthRecordService _fetalGrowthRecordService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fetalGrowthRecordService"></param>
        public FetalGrowthRecordController(IFetalGrowthRecordService fetalGrowthRecordService)
        {
            _fetalGrowthRecordService = fetalGrowthRecordService;
        }

        [HttpGet("/api/v1/MotherInfo/{motherInfoId}/[controller]")]
        public async Task<IActionResult> GetAllFetalGrowthRecordsByUserId([FromRoute] Guid motherInfoId)
        {
            var response = (await _fetalGrowthRecordService.GetAllFetalGrowthRecordsByMotherInfoId(motherInfoId)).Select(x => new SelectFetalGrowthRecordResponse
            {
                Id = x.Id,
                PregnancyRecordId = x.PregnancyRecordId,
                Name = x.Name,
                Unit = x.Unit,
                Description = x.Description,
                Week = x.Week,
                Value = x.Value,
                Note = x.Note,
            });

            if (response.Any())
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = response
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });
        }

        [HttpGet("/api/v1/PregnancyRecord/{pregnancyRecordId}/[controller]")]
        public async Task<IActionResult> GetFetalGrowthRecordById([FromRoute] Guid pregnancyRecordId, [FromQuery] int? week)
        {
            var response = (await _fetalGrowthRecordService.GetFetalGrowthRecordById(pregnancyRecordId, week)).Select(x => new SelectFetalGrowthRecordResponse
            {
                Id = x.Id,
                PregnancyRecordId = x.PregnancyRecordId,
                Name = x.Name,
                Unit = x.Unit,
                Description = x.Description,
                Week = x.Week,
                Value = x.Value,
                Note = x.Note,
            });

            if (response.Any())
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = response
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
        public async Task<IActionResult> CreateFetalGrowthRecord([FromBody] CreateFetalGrowthRecordRequest request)
        {
            var response = await _fetalGrowthRecordService.CreateFetalGrowthRecord(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFetalGrowthRecord([FromBody] UpdateFetalGrowthRecordRequest request)
        {
            var response = await _fetalGrowthRecordService.UpdateFetalGrowthRecord(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFetalGrowthRecord([FromRoute] Guid id)
        {
            var response = (await _fetalGrowthRecordService.DeleteFetalGrowthRecord(id));

            if (!response) return BadRequest(new
            {
                Success = false,
                MessageId = Messages.E00000,
                Message = Messages.GetMessageById(Messages.E00000),
            });

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
            });
        }

    }
}
