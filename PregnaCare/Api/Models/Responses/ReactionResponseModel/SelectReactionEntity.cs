namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class SelectReactionEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserAvatarUrl {  get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
