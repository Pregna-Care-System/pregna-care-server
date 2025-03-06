using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.PregnancyRecordRequestModel
{
    public class UpdatePregnancyRecordRequest : AbstractApiRequest
    {
        public string? BabyName { get; set; }
        public DateOnly? PregnancyStartDate { get; set; }
        public DateOnly? ExpectedDueDate { get; set; }
        public string? BabyGender { get; set; }
        public string? ImageUrl { get; set; }
    }
}
