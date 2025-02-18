    using PregnaCare.Common.Api;
    using PregnaCare.Core.Models;

    namespace PregnaCare.Api.Models.Responses
    {
        public class ReminderTypeListResponse : AbstractApiResponse<IEnumerable<ReminderType>>
        {
            public override IEnumerable<ReminderType> Response { get; set ; }
        }
    }
