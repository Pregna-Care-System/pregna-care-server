using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class FeatureListResponse : AbstractApiResponse<IEnumerable<Feature>>
    {
        public override IEnumerable<Feature> Response { get ; set ; }
    }
}
