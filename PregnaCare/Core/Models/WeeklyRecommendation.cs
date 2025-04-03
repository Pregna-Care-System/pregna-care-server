namespace PregnaCare.Core.Models
{
    public class WeeklyRecommendation
    {
        public Guid Id { get; set; }
        public Guid PregnancyRecordId { get; set; }
        public int Week { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string NutritionalAdvice { get; set; } = string.Empty;
        public string ExerciseRecommendation { get; set; } = string.Empty;
        public string HealthConcerns { get; set; } = string.Empty;
        public string BabyDevelopment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual PregnancyRecord PregnancyRecord { get; set; }
    }
}
