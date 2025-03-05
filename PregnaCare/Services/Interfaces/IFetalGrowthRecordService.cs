using PregnaCare.Api.Models.Requests.FetalGrowthRecordRequestModel;
using PregnaCare.Api.Models.Responses.FetalGrowthRecordResponseModel;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IFetalGrowthRecordService
    {
        Task<List<FetalGrowthRecord>> GetAllFetalGrowthRecordsByMotherInfoId(Guid motherInfoId);
        Task<List<FetalGrowthRecord>> GetFetalGrowthRecordById(Guid pregnancyRecordId, int? week);
        Task<CreateFetalGrowthRecordResponse> CreateFetalGrowthRecord(CreateFetalGrowthRecordRequest request);
        Task<UpdateFetalGrowthRecordResponse> UpdateFetalGrowthRecord(UpdateFetalGrowthRecordRequest request);
        Task<bool> DeleteFetalGrowthRecord(Guid id);
    }
}