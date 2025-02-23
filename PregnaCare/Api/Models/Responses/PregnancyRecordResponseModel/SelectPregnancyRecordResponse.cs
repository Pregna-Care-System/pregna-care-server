namespace PregnaCare.Api.Models.Responses.PregnancyRecordResponseModel
{
    public class SelectPregnancyRecordResponse
    {
        public Guid Id { get; set; }
        public string BabyName { get; set; }
        public DateOnly PregnancyStartDate { get; set; }
        public DateOnly ExpectedDueDate { get; set; }
        public string BabyGender { get; set; }
        public string ImageUrl { get; set; }
        public int TotalWeeks { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public GestationalAgeResponse GestationalAgeResponse { get; set; }
    }
}
