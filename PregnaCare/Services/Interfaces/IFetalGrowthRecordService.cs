using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IFetalGrowthRecordService
    {
        Task<List<FetalGrowthRecord>> GetAllFetalGrowthRecordsByUserId(Guid userId);
        Task<List<FetalGrowthRecord>> GetFetalGrowthRecordById(Guid pregnancyRecordId);
        Task<CreateFetalGrowthRecordResponse> CreateFetalGrowthRecord(CreateFetalGrowthRecordRequest request);
        Task<UpdateFetalGrowthRecordResponse> UpdateFetalGrowthRecord(UpdateFetalGrowthRecordRequest request);
        Task<bool> DeleteFetalGrowthRecord(Guid id);
    }
}