namespace PregnaCare.Api.Models.Responses.ReactionResponseModel
{
    public class SelectReactionEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public string Type { get; set; }
    }
}
