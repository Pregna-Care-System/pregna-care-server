using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IFeatureService
    {
        Task<FeatureResponse> AddFeatureAsync(FeatureRequest request);
        Task <FeatureListResponse> GetAllFeaturesAsync ();
        Task <FeatureResponse> GetFeatureById(Guid id);
        Task<FeatureResponse> Update (Guid id, FeatureRequest request);
        Task Delete (Guid id);

    }
}
