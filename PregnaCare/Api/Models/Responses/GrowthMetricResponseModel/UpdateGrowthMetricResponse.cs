using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.GrowthMetricResponseModel
{
    public class UpdateGrowthMetricResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
