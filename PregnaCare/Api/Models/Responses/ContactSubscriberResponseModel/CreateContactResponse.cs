using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.ContactSubscriberResponseModel
{
    public class CreateContactResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
