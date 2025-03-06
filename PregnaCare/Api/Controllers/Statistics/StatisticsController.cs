using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Services.Interfaces;

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
        /// <param name="statisticsService"></param>
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

        [HttpGet("MembershipStats")]
        public async Task<IActionResult> GetMembershipStats()
        {
            var result = await _statisticsService.GetMembershipStatsAsync();
            if (result.Count > 0)
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = result
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });
        }

        [HttpGet("RecentTransactions")]
        public async Task<IActionResult> GetRecentTransactions([FromQuery] int? offset = 0, [FromQuery] int? limit = 10)
        {

            if (offset < 0 || limit < 1)
            {
                return BadRequest(
                    new
                    {
                        Success = false,
                        MessageId = Messages.E00000,
                        Message = "Offset must be non-negative and limit must be greater than zero.",
                    });
            }

            (int count, offset, limit, List<TransactionStatsResponse> responseList) = await _statisticsService.GetRecentTransactionsAsync(offset.Value, limit.Value);

            if (responseList.Count > 0)
            {
                return Ok(new
                {
                    Success = true,
                    MessageId = Messages.I00001,
                    Message = Messages.GetMessageById(Messages.I00001),
                    Response = new
                    {
                        TotalRecords = count,
                        Offset = offset,
                        Limit = limit,
                        Transactions = responseList
                    }
                });
            }

            return NotFound(new
            {
                Success = false,
                MessageId = Messages.E00013,
                Message = Messages.GetMessageById(Messages.E00013),
            });
        }

        [HttpGet("Revenue")]
        public async Task<ActionResult<List<RevenueStatsResponse>>> GetTotalRevenue()
        {
            var responseList = await _statisticsService.GetTotalRevenueAsync();

            if (responseList.Count > 0)
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

        [HttpGet("NewMember")]
        public async Task<IActionResult> GetNewMembers([FromQuery] int? year, [FromQuery] int? month, [FromQuery] int? week)
        {
            var responseList = await _statisticsService.GetNewMembersAsync(year, month, week);

            if (responseList.Count > 0)
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

        [HttpGet("/PregnancyRecord/{pregnancyRecordId}/FetalGrowthStats")]
        public async Task<IActionResult> GetFetalGrowthStats([FromRoute] Guid pregnancyRecordId)
        {
            var response = await _statisticsService.GetFetalGrowthStatsResponse(pregnancyRecordId);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }
    }
}