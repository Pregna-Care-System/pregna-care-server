using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Requests.FeedBackRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;
using PregnaCare.Api.Models.Responses.FeedbackResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IFeedBackService
    {
        Task<FeedbackResponse> AddFeedbackAsync(FeedbackRequest request, Guid userId);
        Task<FeedbackResponseList> GetAllFeedbackAsync();
        Task<FeedbackResponse> GetFeedbackByIdAsync(Guid id);
        Task Delete(Guid id);
    }
}
