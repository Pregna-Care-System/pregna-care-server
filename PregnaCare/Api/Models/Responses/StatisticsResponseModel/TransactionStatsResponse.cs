namespace PregnaCare.Api.Models.Responses.StatisticsResponseModel
{
    public class TransactionStatsResponse
    {
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string MembershipPlan { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
        public DateTime BuyDate { get; set; }
    }
}
