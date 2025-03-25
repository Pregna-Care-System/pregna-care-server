using Microsoft.AspNetCore.Mvc;
using PregnaCare.Documents;
using PregnaCare.Services.Interfaces;
using QuestPDF.Fluent;

namespace PregnaCare.Api.Controllers.Report
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public ReportController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateAdminReport()
        {
            var memberStats = await _statisticsService.GetMemberStatisticsAsync();
            var userStats = await _statisticsService.GetUserStatisticsAsync();
            var transactionStats = await _statisticsService.GetTotalTransactionStatisticsAsync();
            var revenueStats = await _statisticsService.GetTotalRevenueStatisticsAsync();
            var membershipStats = await _statisticsService.GetMembershipStatsAsync();
            var recentTransactions = (await _statisticsService.GetRecentTransactionsAsync(0, 10)).Item4;
            var revenueData = await _statisticsService.GetTotalRevenueAsync();
            var monthlyNewMembers = await _statisticsService.GetMonthlyNewMembersAsync();

            var document = new AdminReportDocument(
                memberStats,
                userStats,
                transactionStats,
                revenueStats,
                membershipStats,
                recentTransactions,
                revenueData,
                monthlyNewMembers
            );

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream, "application/pdf", "AdminReport.pdf");
        }
    }
}
