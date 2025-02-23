using PregnaCare.Api.Models.Requests.CommunityRequestModel;
using PregnaCare.Api.Models.Responses.CommunityResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface ICommunityService
    {
        Task<IEnumerable<CommunityResponse>> GetAllCommunities();
        Task<CommunityResponse> GetCommunityById(Guid id);
        Task<CommunityResponse> CreateCommunity(CommunityRequest request);
        Task<CommunityResponse> UpdateCommunity(CommunityRequest request);
        Task<bool> DeleteCommunity(Guid id);
    }
}
