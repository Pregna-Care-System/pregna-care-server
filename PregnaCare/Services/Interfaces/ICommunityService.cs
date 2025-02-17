using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
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
