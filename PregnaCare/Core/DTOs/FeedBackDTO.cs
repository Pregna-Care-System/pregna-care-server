namespace PregnaCare.Core.DTOs
{
    public class FeedBackDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string email { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
