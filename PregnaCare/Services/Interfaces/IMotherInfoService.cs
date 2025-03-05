using PregnaCare.Api.Models.Requests.MotherInfoModel;
using PregnaCare.Api.Models.Responses.MotherInfoResponseModel;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IMotherInfoService
    {
        public IEnumerable<MotherInfo> GetAllMotherInfosByUserId(Guid userId);
        public Task<CreateMotherInfoResponse> CreateMotherInfoAsync(CreateMotherInfoRequest request);
    }
}
