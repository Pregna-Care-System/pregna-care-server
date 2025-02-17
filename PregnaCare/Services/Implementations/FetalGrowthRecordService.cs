using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class FetalGrowthRecordService : IFetalGrowthRecordService
    {
        private readonly PregnaCareAppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<FetalGrowthRecord, Guid> _fetalGrowthRecordRepository;
        private readonly IGenericRepository<User, Guid> _userRepository;
        private readonly IGenericRepository<PregnancyRecord, Guid> _pregnancyRecordRepository;
        private readonly IGrowthAlertService _growthAlertService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="growthAlertService"></param>
        public FetalGrowthRecordService(PregnaCareAppDbContext context, IUnitOfWork unitOfWork, IGrowthAlertService growthAlertService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _fetalGrowthRecordRepository = _unitOfWork.GetRepository<FetalGrowthRecord, Guid>();
            _userRepository = _unitOfWork.GetRepository<User, Guid>();
            _pregnancyRecordRepository = _unitOfWork.GetRepository<PregnancyRecord, Guid>();
            _growthAlertService = growthAlertService;
        }

        public async Task<CreateFetalGrowthRecordResponse> CreateFetalGrowthRecord(CreateFetalGrowthRecordRequest request)
        {
            var response = new CreateFetalGrowthRecordResponse { Success = false };

            var user = (await _userRepository.FindWithIncludesAsync(x => x.Id == request.UserId && x.IsDeleted == false,
                x => x.PregnancyRecords)).FirstOrDefault();
            if (user == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            var pregnancyRecord = user.PregnancyRecords.FirstOrDefault(x => x.Id == request.PregnancyRecordId);
            if (pregnancyRecord == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            var isExisted = _context.FetalGrowthRecords.AsNoTracking().FirstOrDefault(x => x.Name == request.Name && x.Week == request.Week && x.IsDeleted == false) != null;

            if (isExisted)
            {
                response.MessageId = Messages.E00014;
                response.Message = Messages.GetMessageById(Messages.E00014);
                return response;
            }

            var fetalGrowthRecord = new FetalGrowthRecord
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Unit = request.Unit ?? string.Empty,
                Description = request.Description ?? string.Empty,
                Week = request.Week ?? 0,
                Value = request.Value ?? 0,
                Note = request.Note ?? string.Empty,
                IsDeleted = false,
                PregnancyRecordId = request.PregnancyRecordId,
            };

            await _fetalGrowthRecordRepository.AddAsync(fetalGrowthRecord);

            var result = await _growthAlertService.CheckGrowthAndCreateAlert(request.UserId, fetalGrowthRecord);

            if (string.IsNullOrEmpty(result))
            {
                response.Success = false;
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<bool> DeleteFetalGrowthRecord(Guid id)
        {
            var entity = (await _fetalGrowthRecordRepository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();
            if (entity == null) return false;

            entity.IsDeleted = true;
            _fetalGrowthRecordRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<FetalGrowthRecord>> GetAllFetalGrowthRecordsByUserId(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var pregnancyRecord = (await _pregnancyRecordRepository.FindWithIncludesAsync(x => x.UserId == userId && x.IsDeleted == false, x => x.User, x => x.FetalGrowthRecords)).FirstOrDefault();
            if (pregnancyRecord == null) return null;

            return pregnancyRecord.FetalGrowthRecords.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id).ThenBy(x => x.PregnancyRecordId).ThenBy(x => x.Week).ToList();
        }

        public async Task<List<FetalGrowthRecord>> GetFetalGrowthRecordById(Guid pregnancyRecordId)
        {
            var pregnancyRecord = (await _pregnancyRecordRepository.FindWithIncludesAsync(x => x.Id == pregnancyRecordId && x.IsDeleted == false,
                x => x.FetalGrowthRecords)).FirstOrDefault();
            if (pregnancyRecord == null) return null;
            return pregnancyRecord.FetalGrowthRecords.OrderBy(x => x.Week).ThenBy(x => x.Name).ToList();
        }

        public async Task<UpdateFetalGrowthRecordResponse> UpdateFetalGrowthRecord(UpdateFetalGrowthRecordRequest request)
        {
            var response = new UpdateFetalGrowthRecordResponse { Success = false };

            var fetalGrowthRecord = (await _fetalGrowthRecordRepository.FindAsync(x => x.Id == request.FetalGrowthRecordId && x.IsDeleted == false)).FirstOrDefault();
            if (fetalGrowthRecord == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            fetalGrowthRecord.Name = request.Name;
            fetalGrowthRecord.Unit = request.Unit ?? string.Empty;
            fetalGrowthRecord.Description = request.Description ?? string.Empty;
            fetalGrowthRecord.Week = request.Week ?? 0;
            fetalGrowthRecord.Value = request.Value ?? 0;
            fetalGrowthRecord.Note = request.Note ?? string.Empty;

            _fetalGrowthRecordRepository.Update(fetalGrowthRecord);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
