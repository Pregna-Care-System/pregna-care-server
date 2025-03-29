namespace PregnaCare.Core.Models
{
    public partial class Reaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid? BlogId { get; set; }

        public Guid? CommentId { get; set; }

        public string Type { get; set; } = string.Empty; // Like, Love, Dislike, etc.

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual Blog? Blog { get; set; }
        public virtual Comment? Comment { get; set; }
        public virtual User User { get; set; }
    }
}
