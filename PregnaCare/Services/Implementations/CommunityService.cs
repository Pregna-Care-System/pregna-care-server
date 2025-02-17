using PregnaCare.Services.Interfaces;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private readonly List<CommunityRequest> _communities = new List<CommunityRequest>();

        public async Task<IEnumerable<CommunityResponse>> GetAllCommunities()
        {
            return await Task.FromResult(_communities.Select(c => new CommunityResponse { Response = c.Name }));
        }

        public async Task<CommunityResponse> GetCommunityById(Guid id)
        {
            var community = _communities.FirstOrDefault(c => c.Id == id);
            if (community == null) return null;
            return await Task.FromResult(new CommunityResponse { Response = community.Name });
        }

        public async Task<CommunityResponse> CreateCommunity(CommunityRequest request)
        {
            request.Id = Guid.NewGuid();
            _communities.Add(request);
            return await Task.FromResult(new CommunityResponse { Success = true, Response = "Community created successfully" });
        }

        public async Task<CommunityResponse> UpdateCommunity(CommunityRequest request)
        {
            var community = _communities.FirstOrDefault(c => c.Id == request.Id);
            if (community == null)
                return new CommunityResponse { Success = false, Response = "Community not found" };

            community.Name = request.Name;
            community.Description = request.Description;

            return await Task.FromResult(new CommunityResponse { Success = true, Response = "Community updated successfully" });
        }

        public async Task<bool> DeleteCommunity(Guid id)
        {
            var community = _communities.FirstOrDefault(c => c.Id == id);
            if (community == null) return false;
            _communities.Remove(community);
            return await Task.FromResult(true);
        }
    }
}
