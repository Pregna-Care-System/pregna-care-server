using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class CreatePregnancyRecordRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }
        public string BabyName { get; set; }
        public DateOnly PregnancyStartDate { get; set; }
        public DateOnly? ExpectedDueDate { get; set; }
        public string? BabyGender { get; set; }
        public string? ImageUrl {  get; set; } 
    }
}
