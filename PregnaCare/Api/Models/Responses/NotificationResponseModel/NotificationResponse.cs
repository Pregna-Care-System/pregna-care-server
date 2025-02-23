using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.NotificationResponseModel
{
    public class NotificationResponse: AbstractApiResponse<Notification>
    {
        public override Notification Response { get; set; }
    }
}
