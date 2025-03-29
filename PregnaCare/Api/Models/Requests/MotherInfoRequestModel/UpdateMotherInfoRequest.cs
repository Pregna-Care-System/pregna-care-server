using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.MotherInfoRequestModel
{
    public class UpdateMotherInfoRequest : AbstractApiRequest
    {
        public string BloodType { get; set; }
        public string HealhStatus { get; set; }
        public string Notes { get; set; }
    }
}
