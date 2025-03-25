using PregnaCare.Api.Models.Requests.ContactSubscriberRequestModel;
using PregnaCare.Api.Models.Responses.ContactSubscriberResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IContactService
    {
        Task<CreateContactResponse> CreateContactAsync(CreateContactRequest request);
        Task<bool> DeleteContactAsync(string email);
        Task<SelectContactResponse> SelectContactAsync();
    }
}
