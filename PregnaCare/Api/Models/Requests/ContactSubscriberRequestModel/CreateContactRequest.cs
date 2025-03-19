using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.ContactSubscriberRequestModel
{
    public class CreateContactRequest : AbstractApiRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
