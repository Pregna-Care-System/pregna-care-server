using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.AccountRequestModel
{
    public class UpdateAccountRequest : AbstractApiRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateOnly? DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
