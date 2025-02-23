using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.ReminderResponseModel
{
    public class ReminderResponse : AbstractApiResponse<Reminder>
    {
        public override Reminder Response { get; set; }
    }
}
