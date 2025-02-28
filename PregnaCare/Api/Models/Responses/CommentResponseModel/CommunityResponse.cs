using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.CommentResponseModel
{
    public class CommentResponse : AbstractApiResponse<Comment>
    {
        public override Comment Response { get; set; }
    }
}
