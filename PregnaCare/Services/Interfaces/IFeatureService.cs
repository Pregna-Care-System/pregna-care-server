using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IFeatureService
    {
        Task<FeatureResponse> AddFeatureAsync(FeatureRequest request);
        Task<FeatureListResponse> GetAllFeaturesAsync();
        Task<FeatureResponse> GetFeatureById(Guid id);
        Task<FeatureResponse> Update(Guid id, FeatureRequest request);
        Task<FeatureResponse> Delete(Guid id);
        Task<List<SelectFeatureResponse>> GetAllFeaturesByUserIdAsync(Guid userId);
    }
}
