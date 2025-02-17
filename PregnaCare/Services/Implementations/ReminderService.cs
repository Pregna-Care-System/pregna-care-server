using Azure;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ReminderService : IReminderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Reminder, Guid> _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ReminderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Reminder, Guid>();
        }
        public async Task CreateReminder(ReminderRequest request)
        {
            var type = new Reminder
            {
               Id = Guid.NewGuid(),
               Description = request.Description,
               ReminderTypeId = request.ReminderTypeId,
               Title = request.Title,
               Status = request.Status,
               ReminderDate = request.ReminderDate,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow,
               IsDeleted = false,
              
            };

            await _repository.AddAsync(type);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReminder(Guid id)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();

            type.IsDeleted = true;
            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<IEnumerable<ReminderResponse>> GetAllReminder()
        {
            var typeList = await _repository.GetAllAsync();

            return typeList.Select(type => new ReminderResponse
            {
                Success = true,
                Response = type
            });
        }

        public async Task<ReminderResponse> GetReminderById(Guid id)
        {
            var reminder = await _repository.GetByIdAsync(id);
            return new ReminderResponse
            {
                Success = true,
                Response = reminder
            };
        }

        public async Task UpdateReminder(Guid id, ReminderRequest request)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();

            type.Description = request.Description;
            type.Title = request.Title;
            type.Status = request.Status;
            type.ReminderTypeId = request.ReminderTypeId;
            type.ReminderDate = request.ReminderDate;
            type.UpdatedAt = DateTime.UtcNow;

            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
