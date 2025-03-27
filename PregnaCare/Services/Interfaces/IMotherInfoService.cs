using PregnaCare.Api.Models.Requests.MotherInfoRequestModel;
using PregnaCare.Api.Models.Responses.MotherInfoResponseModel;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IMotherInfoService
    {
        public IEnumerable<MotherInfo> GetAllMotherInfosByUserId(Guid userId);
        public Task<CreateMotherInfoResponse> CreateMotherInfoAsync(CreateMotherInfoRequest request);
        public Task<UpdateMotherInfoResponse> UpdateMotherInfoAsync(Guid motherInfoId, UpdateMotherInfoRequest request);
    }
}
