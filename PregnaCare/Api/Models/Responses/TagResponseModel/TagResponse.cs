using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.TagResponseModel
{
    public class TagResponse : AbstractApiResponse<Tag>
    {
        public override Tag Response { get; set; }

    }
}
