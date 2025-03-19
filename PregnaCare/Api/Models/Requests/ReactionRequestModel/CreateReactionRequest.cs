using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.ReactionRequestModel
{
    public class CreateReactionRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }
        public Guid? BlogId { get; set; }
        public Guid? CommentId { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
