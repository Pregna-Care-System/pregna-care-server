using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.FeedbackResponseModel
{
    public class FeedbackResponseList : AbstractApiResponse<IEnumerable<FeedBackDTO>>
    {
        public override IEnumerable<FeedBackDTO> Response { get; set; }
    }
}
