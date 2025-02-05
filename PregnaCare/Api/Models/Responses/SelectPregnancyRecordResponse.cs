namespace PregnaCare.Api.Models.Responses
{
    public class SelectPregnancyRecordResponse
    {
        public Guid Id { get; set; }
        public string BabyName { get; set; }
        public DateOnly PregnancyStartDate { get; set; }
        public DateOnly ExpectedDueDate { get; set; }
        public string BabyGender { get; set; }
        public string ImageUrl { get; set; }
    }
}
