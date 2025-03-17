using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class CreateReactionResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
