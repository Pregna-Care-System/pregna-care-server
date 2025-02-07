namespace PregnaCare.Api.Models.Responses
{
    public class SelectPregnancyRecordResponse
    {
        public Guid Id { get; set; }
        public string MotherName { get; set; }
        public DateOnly MotherDateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string HealhStatus { get; set; }
        public string Notes { get; set; }
        public string BabyName { get; set; }
        public DateOnly PregnancyStartDate { get; set; }
        public DateOnly ExpectedDueDate { get; set; }
        public string BabyGender { get; set; }
        public string ImageUrl { get; set; }
    }
}
