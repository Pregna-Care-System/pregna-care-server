using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FetalGrowthRecordResponseModel
{
    public class UpdateFetalGrowthRecordResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
