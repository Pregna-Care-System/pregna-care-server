using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.StatisticsResponseModel
{
    public class FetalGrowthStatsResponse : AbstractApiResponse<List<FetalGrowthStats>>
    {
        public override List<FetalGrowthStats> Response { get; set; }
    }

    public class FetalGrowthStats
    {
        public string MetricName { get; set; }
        public int Week { get; set; }
        public List<MetricResponse> MetricResponseList { get; set; }
    }

    public class MetricResponse
    {
        public double CurrentValue { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}
