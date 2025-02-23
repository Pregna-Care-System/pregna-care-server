using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.ReminderResponseModel
{
    public class ReminderListResponse : AbstractApiResponse<IEnumerable<Reminder>>
    {
        public override IEnumerable<Reminder> Response { get; set; }
    }
}
