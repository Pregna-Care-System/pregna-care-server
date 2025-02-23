namespace PregnaCare.Api.Models.Responses.StatisticsResponseModel
{
    public class RevenueStatsResponse
    {
        public string Year { get; set; } 
        public List<decimal> TotalRevenueByMonth { get; set; }
    }
}
