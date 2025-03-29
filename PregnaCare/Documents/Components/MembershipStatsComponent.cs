using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PregnaCare.Documents.Components
{
    public class MembershipStatsComponent : IComponent
    {
        private readonly List<MembershipStatsResponse> _stats;

        public MembershipStatsComponent(List<MembershipStatsResponse> stats)
        {
            _stats = stats;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    _ = header.Cell().Text("Plan Name").Bold();
                    _ = header.Cell().Text("Users").Bold();
                });

                foreach (var stat in _stats)
                {
                    _ = table.Cell().Text(stat.Name);
                    _ = table.Cell().Text(stat.Users.ToString());
                }
            });
        }
    }
}
