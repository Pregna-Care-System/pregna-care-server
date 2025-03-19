using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class UpdateReactionResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
