using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.CommunityResponseModel
{
    public class CommunityResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
