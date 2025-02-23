using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.NotificationResponseModel
{
    public class NotificationListResponse: AbstractApiResponse<IEnumerable<Notification>>
    {
        public override IEnumerable<Notification> Response { get; set; }
    }
}
