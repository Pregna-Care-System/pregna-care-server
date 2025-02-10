using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.GrowthMetric
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GrowthMetricController : ControllerBase
    {
        private readonly IGrowthMetricService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public GrowthMetricController(IGrowthMetricService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = (await _service.GetAllGrowthMetrics()).Select(x => new
            {
                Name = x.Name,
                Unit = x.Unit,
                Description = x.Description,
                MinValue = x.MinValue,
                MaxValue = x.MaxValue,
                Week = x.Week,
            });

            if (response == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var x = (await _service.GetGrowthMetricById(id));
            if (x == null) return NotFound(Messages.GetMessageById(Messages.E00013));
            return Ok(new
            {
                Name = x.Name,
                Unit = x.Unit,
                Description = x.Description,
                MinValue = x.MinValue,
                MaxValue = x.MaxValue,
                Week = x.Week,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGrowthMetricRequest request)
        {
            var response = (await _service.CreateGrowthMetric(request));

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateGrowthMetricRequest request)
        {
            var response = (await _service.UpdateGrowthMetric(request));

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = (await _service.DeleteGrowthMetric(id));

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
