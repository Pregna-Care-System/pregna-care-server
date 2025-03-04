using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.TagResponseModel
{
    public class TagListResponse : AbstractApiResponse<IEnumerable<Tag>>
    {
        public override IEnumerable<Tag> Response { get; set; }

    }
}
