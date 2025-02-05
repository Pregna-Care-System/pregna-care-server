using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IPregnancyRecordService
    {
        Task<List<PregnancyRecord>> GetAllPregnancyRecords(Guid userId);
        Task<PregnancyRecord> GetPregnancyRecordById(Guid userId, Guid pregnancyRecordId);
        Task<CreatePregnancyRecordResponse> CreatePregnancyRecord(CreatePregnancyRecordRequest request);
        Task<UpdatePregnancyRecordResponse> UpdatePregnancyRecord(UpdatePregnancyRecordRequest request);
        Task<bool> DeletePregnancyRecord(Guid pregnancyRecordId);
    }
}
