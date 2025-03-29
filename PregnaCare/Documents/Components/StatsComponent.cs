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
                _ = column.Item().Text(_title).Bold();
                _ = column.Item().Text($"Total: {_stats.Total}");
                _ = column.Item().Text($"Change: {_stats.PercentageChange}%");
            });
        }
    }
}
