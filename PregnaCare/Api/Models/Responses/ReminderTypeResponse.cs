using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class ReminderTypeResponse : AbstractApiResponse<ReminderType>
    {
        public override ReminderType Response { get; set; }
    }
}
