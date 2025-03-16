using PregnaCare.Api.Models.Requests.FAQRequestModel;
using PregnaCare.Api.Models.Responses.FAQResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IFAQService
    {
        Task<CreateFAQResponse> CreateFAQAsync(CreateFAQRequest request);
        Task<UpdateFAQResponse> UpdateFAQAsync(Guid id, UpdateFAQRequest request);
        Task<DeleteFAQResponse> DeleteFAQAsync(Guid id);
    }
}
