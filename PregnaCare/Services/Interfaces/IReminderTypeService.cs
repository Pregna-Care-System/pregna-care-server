﻿using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IReminderTypeService
    {
        Task CreateReminderType(ReminderTypeRequest request);
        Task<IEnumerable<ReminderTypeResponse>> GetAllReminderType();
        Task UpdateReminderType(Guid id, ReminderTypeRequest request);
        Task DeleteReminderType(Guid id);
    }
}
