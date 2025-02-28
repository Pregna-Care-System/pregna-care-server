using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.BlogResponseModel
{
    public class BlogListResponse : AbstractApiResponse<IEnumerable<Blog>>
    {
        public override IEnumerable<Blog> Response { get; set; }

    }
}
