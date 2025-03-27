using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IGrowthAlertService
    {
        Task<string> CheckGrowthAndCreateAlert(Guid userId, FetalGrowthRecord record);
        Task<List<GrowthAlert>> GetGrowthAlerts(Guid userId);
        Task<List<GrowthAlert>> GetGrowthAlertsByPregnancyRecordId(Guid PregnancyRecordId);
        Task<bool> UpdateStatusGrowthAlert(Guid growthAlertId, string status);
        Task<List<GrowthAlert>> GetFetalGrowthRecordsToSendNotification();
    }
}
