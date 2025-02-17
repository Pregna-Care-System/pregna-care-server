using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses
{
    public class CreateFetalGrowthRecordResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
