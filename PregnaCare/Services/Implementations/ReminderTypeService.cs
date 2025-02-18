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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                TypeName = request.TypeName,

            };

            await _repository.AddAsync(type);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReminderType(Guid id)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();

            type.IsDeleted = true;
            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<ReminderTypeListResponse> GetAllReminderType()
        {
            var typeList = await _repository.GetAllAsync();

            return new ReminderTypeListResponse
            {
                Success = true,
                Response = typeList
            };
        }

        public async Task UpdateReminderType(Guid id, ReminderTypeRequest request)
        {
            var type = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();

            type.TypeName = request.TypeName;
            type.Description = request.Description;
            type.UpdatedAt = DateTime.UtcNow;

            _repository.Update(type);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
