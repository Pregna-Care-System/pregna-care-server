using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PregnaCare.Documents.Components
{
    public class RecentTransactionsComponent : IComponent
    {
        private readonly List<TransactionStatsResponse> _transactions;

        public RecentTransactionsComponent(List<TransactionStatsResponse> transactions)
        {
            _transactions = transactions;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Tên
                    columns.RelativeColumn(2); // Gói thành viên
                    columns.RelativeColumn(); // Giá
                    columns.RelativeColumn(); // Ngày mua
                    columns.RelativeColumn(); // Trạng thái
                });

                table.Header(header =>
                {
                    header.Cell().Text("Full Name").Bold();
                    header.Cell().Text("Membership Plan").Bold();
                    header.Cell().Text("Price").Bold();
                    header.Cell().Text("Buy Date").Bold();
                    header.Cell().Text("Status").Bold();
                });

                foreach (var transaction in _transactions)
                {
                    table.Cell().Text(transaction.FullName);
                    table.Cell().Text(transaction.MembershipPlan);
                    table.Cell().Text(transaction.Price);
                    table.Cell().Text(transaction.BuyDate.ToString("yyyy-MM-dd"));
                    table.Cell().Text(transaction.Status);
                }
            });
        }
    }
}
