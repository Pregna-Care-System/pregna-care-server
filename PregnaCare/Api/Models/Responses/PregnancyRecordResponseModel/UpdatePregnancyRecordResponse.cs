using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.PregnancyRecordResponseModel
{
    public class UpdatePregnancyRecordResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
