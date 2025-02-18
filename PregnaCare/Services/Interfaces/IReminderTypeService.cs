using PregnaCare.Api.Models.Requests.ReminderRequestModel;
using PregnaCare.Api.Models.Responses.ReminderResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IReminderTypeService
    {
        Task CreateReminderType(ReminderTypeRequest request);
        Task<ReminderTypeListResponse> GetAllReminderType();
        Task UpdateReminderType(Guid id, ReminderTypeRequest request);
        Task DeleteReminderType(Guid id);
    }
}
