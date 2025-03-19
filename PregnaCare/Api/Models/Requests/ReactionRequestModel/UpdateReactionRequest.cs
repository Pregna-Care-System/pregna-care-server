using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.ReactionRequestModel
{
    public class UpdateReactionRequest : AbstractApiRequest
    {
        public string Type { get; set; } = string.Empty;
    }
}
