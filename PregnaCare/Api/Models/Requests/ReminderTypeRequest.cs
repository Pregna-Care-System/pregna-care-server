using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class ReminderTypeRequest: AbstractApiRequest
    {
        public string TypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;



    }
}
