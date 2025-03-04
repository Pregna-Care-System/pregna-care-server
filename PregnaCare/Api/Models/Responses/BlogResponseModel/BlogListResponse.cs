using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.BlogResponseModel
{
    public class BlogListResponse : AbstractApiResponse<IEnumerable<BlogDTO>>
    {
        public override IEnumerable<BlogDTO> Response { get; set; }

    }
}
