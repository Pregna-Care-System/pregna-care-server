using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses.FeedbackResponseModel
{
    public class FeedbackResponse : AbstractApiResponse<FeedBackDTO>
    {
        public override FeedBackDTO Response { get; set; }
    }
}
