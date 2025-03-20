using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PregnaCare.Documents.Components
{
    public class StatsComponent : IComponent
    {
        private readonly string _title;
        private readonly StatsResponse _stats;

        public StatsComponent(string title, StatsResponse stats)
        {
            _title = title;
            _stats = stats;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text(_title).Bold();
                column.Item().Text($"Total: {_stats.Total}");
                column.Item().Text($"Change: {_stats.PercentageChange}%");
            });
        }
    }
}
