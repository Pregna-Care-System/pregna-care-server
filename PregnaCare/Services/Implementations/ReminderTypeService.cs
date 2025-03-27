using PregnaCare.Api.Models.Requests.ReminderRequestModel;
using PregnaCare.Api.Models.Responses.ReminderResponseModel;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ReminderTypeService : IReminderTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ReminderType, Guid> _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ReminderTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<ReminderType, Guid>();
        }
        public async Task CreateReminderType(ReminderTypeRequest request)
        {
            var type = new ReminderType
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                TypeName = request.TypeName,

            };

            await _repository.AddAsync(type);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReminderType(Guid id)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();
            if (type == null)
            {
                throw new KeyNotFoundException("Reminder Type not found.");
            }

            type.IsDeleted = true;
            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<ReminderTypeListResponse> GetAllReminderType()
        {
            var typeList = await _repository.FindAsync(x => x.IsDeleted == false);

            return new ReminderTypeListResponse
            {
                Success = typeList.Any(),
                Response = typeList.Any() ? typeList : new List<ReminderType>()
            };
        }

        public async Task UpdateReminderType(Guid id, ReminderTypeRequest request)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();

            type.TypeName = request.TypeName;
            type.Description = request.Description;
            type.UpdatedAt = DateTime.Now;

            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
