using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.ReminderRequestModel
{
    public class ReminderRequest : AbstractApiRequest
    {
        public Guid ReminderTypeId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? ReminderDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
