using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.FetalGrowthRecordRequestModel;
using PregnaCare.Api.Models.Responses.FetalGrowthRecordResponseModel;
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
        private readonly IGenericRepository<MotherInfo, Guid> _motherInfoRepository;
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
            _motherInfoRepository = _unitOfWork.GetRepository<MotherInfo, Guid>();
        }

        public async Task<CreateFetalGrowthRecordResponse> CreateFetalGrowthRecord(CreateFetalGrowthRecordRequest request)
        {
            var response = new CreateFetalGrowthRecordResponse { Success = false };

            var user = (await _userRepository.FindWithIncludesAsync(x => x.Id == request.UserId && x.IsDeleted == false,
                x => x.MotherInfo, x => x.MotherInfo.PregnancyRecords)).FirstOrDefault();
            if (user == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            var pregnancyRecord = user.MotherInfo.PregnancyRecords.FirstOrDefault(x => x.Id == request.PregnancyRecordId);
            if (pregnancyRecord == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            var createdRecords = new List<FetalGrowthRecord>();

            var fetalRecords = _context.FetalGrowthRecords.AsNoTracking().AsQueryable();
            foreach (var createEntity in request.CreateFetalGrowthRecordEntities)
            {
                var isExisted = fetalRecords.FirstOrDefault(x => x.PregnancyRecordId == request.PregnancyRecordId && x.Name == createEntity.Name && x.Week == request.Week && x.IsDeleted == false) != null;

                if (isExisted)
                {
                    response.MessageId = Messages.E00014;
                    response.Message = Messages.GetMessageById(Messages.E00014);
                    return response;
                }

                var fetalGrowthRecord = new FetalGrowthRecord
                {
                    Id = Guid.NewGuid(),
                    Name = createEntity.Name,
                    Unit = createEntity.Unit ?? string.Empty,
                    Description = createEntity.Description ?? string.Empty,
                    Week = request.Week ?? 0,
                    Value = createEntity.Value ?? 0,
                    Note = createEntity.Note ?? string.Empty,
                    IsDeleted = false,
                    PregnancyRecordId = request.PregnancyRecordId,
                };

                await _fetalGrowthRecordRepository.AddAsync(fetalGrowthRecord);
                createdRecords.Add(fetalGrowthRecord);
            }

            await _unitOfWork.SaveChangesAsync();
            await _growthAlertService.ProcessBatchFetalGrowthRecords(request.UserId, createdRecords);

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

        public async Task<List<FetalGrowthRecord>> GetAllFetalGrowthRecordsByMotherInfoId(Guid motherInfoId)
        {
            var motherInfo = await _motherInfoRepository.GetByIdAsync(motherInfoId);
            if (motherInfo == null) return new();

            var pregnancyRecord = (await _pregnancyRecordRepository.FindWithIncludesAsync(x => x.MotherInfoId == motherInfoId && x.IsDeleted == false, x => x.FetalGrowthRecords)).FirstOrDefault();
            if (pregnancyRecord == null) return new();

            return pregnancyRecord.FetalGrowthRecords.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id).ThenBy(x => x.PregnancyRecordId).ThenBy(x => x.Week).ToList();
        }

        public async Task<List<FetalGrowthRecord>> GetFetalGrowthRecordById(Guid pregnancyRecordId, int? week)
        {
            var pregnancyRecord = (await _pregnancyRecordRepository.FindWithIncludesAsync(x => x.Id == pregnancyRecordId && x.IsDeleted == false,
                x => x.FetalGrowthRecords)).FirstOrDefault();

            if (pregnancyRecord == null || pregnancyRecord.FetalGrowthRecords == null) return new();

            if (week.HasValue)
            {
                return pregnancyRecord.FetalGrowthRecords
                    .Where(x => x.Week == week.Value)
                    .OrderBy(x => x.Week)
                    .ThenBy(x => x.Name)
                    .ToList();
            }

            return pregnancyRecord.FetalGrowthRecords
                .OrderBy(x => x.Week)
                .ThenBy(x => x.Name)
                .ToList();
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
