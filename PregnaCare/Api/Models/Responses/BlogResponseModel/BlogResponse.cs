using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.BlogResponseModel
{
    public class BlogResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }

    }
}
