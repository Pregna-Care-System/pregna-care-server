using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.CommunityRequestModel
{
    public class CommunityRequest : AbstractApiRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
