using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using PregnaCare.Documents.Components;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PregnaCare.Documents
{

    public class AdminReportDocument : IDocument
    {
        private readonly StatsResponse _memberStats;
        private readonly StatsResponse _userStats;
        private readonly StatsResponse _transactionStats;
        private readonly StatsResponse _revenueStats;
        private readonly List<MembershipStatsResponse> _membershipStats;
        private readonly List<TransactionStatsResponse> _recentTransactions;
        private readonly List<RevenueStatsResponse> _revenueData;
        private readonly List<NewMembersDataPointResponse> _monthlyNewMembers;

        public AdminReportDocument(
            StatsResponse memberStats,
            StatsResponse userStats,
            StatsResponse transactionStats,
            StatsResponse revenueStats,
            List<MembershipStatsResponse> membershipStats,
            List<TransactionStatsResponse> recentTransactions,
            List<RevenueStatsResponse> revenueData,
            List<NewMembersDataPointResponse> monthlyNewMembers)
        {
            _memberStats = memberStats;
            _userStats = userStats;
            _transactionStats = transactionStats;
            _revenueStats = revenueStats;
            _membershipStats = membershipStats;
            _recentTransactions = recentTransactions;
            _revenueData = revenueData;
            _monthlyNewMembers = monthlyNewMembers;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                _ = page.Header()
                    .AlignCenter()
                    .Text($"Monthly Admin Report - {DateTime.Now.ToString("MMMM yyyy")}")
                    .Bold().FontSize(20);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        _ = column.Item().Text("Report Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        _ = column.Item().PaddingTop(10).Text("Overview").Bold().FontSize(16);
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Component(new StatsComponent("Total Members", _memberStats));
                            row.RelativeItem().Component(new StatsComponent("Total Users", _userStats));
                            row.RelativeItem().Component(new StatsComponent("Total Transactions", _transactionStats));
                            row.RelativeItem().Component(new StatsComponent("Total Revenue", _revenueStats));
                        });

                        _ = column.Item().PaddingTop(10).Text("Membership Plan Distribution").Bold().FontSize(16);
                        column.Item().Component(new MembershipStatsComponent(_membershipStats));

                        _ = column.Item().PaddingTop(10).Text("Recent Transactions").Bold().FontSize(16);
                        column.Item().Component(new RecentTransactionsComponent(_recentTransactions));

                        //_ = column.Item().PaddingTop(10).Text("Monthly Revenue").Bold().FontSize(16);
                        //column.Item().Component(new RevenueChartComponent(_revenueData));

                        //_ = column.Item().PaddingTop(10).Text("Monthly New Members").Bold().FontSize(16);
                        //column.Item().Component(new NewMembersChartComponent(_monthlyNewMembers));
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        _ = x.Span("Page ");
                        _ = x.CurrentPageNumber();
                    });
            });
        }
    }
}