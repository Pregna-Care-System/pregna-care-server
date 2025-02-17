using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class ReminderResponse : AbstractApiResponse<Reminder>
    {
        public override Reminder Response { get; set; }
    }
}
