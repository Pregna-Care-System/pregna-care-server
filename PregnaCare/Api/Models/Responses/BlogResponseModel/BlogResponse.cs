using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.BlogResponseModel
{
    public class BlogResponse : AbstractApiResponse<Blog>
    {
        public override Blog Response { get; set; }

    }
}
