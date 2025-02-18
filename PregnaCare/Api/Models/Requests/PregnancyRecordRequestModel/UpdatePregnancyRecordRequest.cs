using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.PregnancyRecordRequestModel
{
    public class UpdatePregnancyRecordRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }
        public Guid PregnancyRecordId { get; set; }
        public string MotherName { get; set; }
        public DateOnly MotherDateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string HealhStatus { get; set; }
        public string Notes { get; set; }
        public string? BabyName { get; set; }
        public DateOnly? PregnancyStartDate { get; set; }
        public DateOnly? ExpectedDueDate { get; set; }
        public string? BabyGender { get; set; }
        public string? ImageUrl { get; set; }
    }
}
