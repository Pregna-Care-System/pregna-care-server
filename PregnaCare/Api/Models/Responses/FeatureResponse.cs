using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class FeatureResponse : AbstractApiResponse<Feature>
    {
        public override Feature Response { get; set; }
    }
}
