namespace PregnaCare.Core.Models
{
    public class FAQ
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual FAQCategory Category { get; set; }
    }
}
