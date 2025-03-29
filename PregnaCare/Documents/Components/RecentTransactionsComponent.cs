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
                    _ = header.Cell().Text("Full Name").Bold();
                    _ = header.Cell().Text("Membership Plan").Bold();
                    _ = header.Cell().Text("Price").Bold();
                    _ = header.Cell().Text("Buy Date").Bold();
                    _ = header.Cell().Text("Status").Bold();
                });

                foreach (var transaction in _transactions)
                {
                    _ = table.Cell().Text(transaction.FullName);
                    _ = table.Cell().Text(transaction.MembershipPlan);
                    _ = table.Cell().Text(transaction.Price);
                    _ = table.Cell().Text(transaction.BuyDate.ToString("yyyy-MM-dd"));
                    _ = table.Cell().Text(transaction.Status);
                }
            });
        }
    }
}
