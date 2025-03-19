using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ContactSubscriberResponseModel
{
    public class SelectContactResponse : AbstractApiResponse<List<SelectContactEntity>>
    {
        public override List<SelectContactEntity> Response { get; set; }
    }

    public class SelectContactEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}