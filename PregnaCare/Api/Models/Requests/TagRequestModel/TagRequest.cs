using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.TagRequestModel
{
    public class TagRequest : AbstractApiRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

    }
}