using Microsoft.AspNetCore.Mvc;
using PregnaCare.Services.Interfaces;
using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using PregnaCare.Common.Constants;

namespace PregnaCare.Api.Controllers.Statistics
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("MemberStatistics")]
        public async Task<IActionResult> GetMemberStatistics()
        {
            var result = await _statisticsService.GetMemberStatisticsAsync();

            if (result.Total == 0)
            {
                return NotFound(new
                {
                    Success = false,
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013),
                });
            }

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = result
            });
        }

        [HttpGet("UserStatistics")]
        public async Task<IActionResult> GetUserStatistics()
        {
            var result = await _statisticsService.GetUserStatisticsAsync();

            if (result.Total == 0)
            {
                return NotFound(new
                {
                    Success = false,
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013),
                });
            }

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = result
            });
        }

        [HttpGet("TransactionStatistics")]
        public async Task<IActionResult> GetTransactionStatistics()
        {
            var result = await _statisticsService.GetTotalTransactionStatisticsAsync();

            if (result.Total == 0)
            {
                return NotFound(new
                {
                    Success = false,
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013),
                });
            }

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = result
            });
        }

        [HttpGet("RevenueStatistics")]
        public async Task<IActionResult> GetRevenueStatistics()
        {
            var result = await _statisticsService.GetTotalRevenueStatisticsAsync();

            if (result.Total == 0)
            {
                return NotFound(new
                {
                    Success = false,
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013),
                });
            }

            return Ok(new
            {
                Success = true,
                MessageId = Messages.I00001,
                Message = Messages.GetMessageById(Messages.I00001),
                Response = result
            });
        }
    }
}
