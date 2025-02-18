using PregnaCare.Api.Models.Requests.ReminderRequestModel;
using PregnaCare.Api.Models.Responses.ReminderResponseModel;
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
        private readonly IReminderRepository _reminderRepo;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ReminderService(IUnitOfWork unitOfWork, IReminderRepository reminderRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Reminder, Guid>();
            _reminderRepo = reminderRepository;
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
                StartTime = request.StartTime,
                EndTime = request.EndTime,
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

        public async Task<ReminderListResponse> GetAllActiveReminders()
        {
            var list = await _reminderRepo.GetActiveRemindersAsync();
            return new ReminderListResponse
            {
                Success = true,
                Response = list
            };
        }

        public async Task<ReminderListResponse> GetAllReminders()
        {
            var reminderList = await _repository.GetAllAsync();
            var activeReminders = reminderList.Where(r => r.IsDeleted == false).OrderBy(x => x.ReminderDate).ToList();

            return new ReminderListResponse
            {
                Success = true,
                Response = activeReminders
            };
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
            type.StartTime = request.StartTime;
            type.EndTime = request.EndTime;
            type.UpdatedAt = DateTime.UtcNow;

            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
