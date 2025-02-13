using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses
{
    public class AccountResponse : AbstractApiResponse<AccountDTO>
    {
        public override AccountDTO Response { get; set; }
    }
}
