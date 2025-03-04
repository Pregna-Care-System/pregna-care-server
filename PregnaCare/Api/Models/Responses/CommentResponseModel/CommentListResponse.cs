using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.CommentResponseModel
{
    public class CommentListResponse : AbstractApiResponse<IEnumerable<Comment>>
    {
        public override IEnumerable<Comment> Response { get; set; }
    }
}
