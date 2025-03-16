using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses.BlogResponseModel
{
    public class SelectDetailBlogResponse : AbstractApiResponse<BlogDTO>
    {
        public override BlogDTO Response { get; set; }
    }
}
