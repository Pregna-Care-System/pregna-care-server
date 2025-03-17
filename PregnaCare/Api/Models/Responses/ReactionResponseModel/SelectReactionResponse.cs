using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class SelectReactionResponse : AbstractApiResponse<List<SelectReactionEntity>>
    {
        public override List<SelectReactionEntity> Response { get; set; }
    }
}
