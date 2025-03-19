using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class DeleteReactionResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
