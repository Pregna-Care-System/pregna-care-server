namespace PregnaCare.Api.Models.Responses.StatisticsResponseModel
{
    public class StatsResponse
    {
        public string Title { get; set; }
        public decimal Total { get; set; }
        public double PercentageChange { get; set; }
        public bool IsIncrease { get; set; }
    }
}
