using PregnaCare.Api.Models.Requests.PregnancyRecordRequestModel;
using PregnaCare.Api.Models.Responses.PregnancyRecordResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class PregnancyRecordService : IPregnancyRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<PregnancyRecord, Guid> _repository;
        private readonly IGenericRepository<MotherInfo, Guid> _motherInfoRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public PregnancyRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<PregnancyRecord, Guid>();
            _motherInfoRepository = _unitOfWork.GetRepository<MotherInfo, Guid>();
        }

        public GestationalAgeResponse CalculateGestationalAge(DateTime lmp)
        {
            var today = DateTime.Today;
            var difference = today - lmp;
            var totalDays = (int)difference.TotalDays;

            var weeks = totalDays / 7;
            var days = totalDays % 7;
            var maxEditableWeek = weeks + (days > 0 ? 1 : 0);

            var estimatedDueDate = lmp.AddDays(280); // 40 weeks

            return new GestationalAgeResponse
            {
                Weeks = weeks == 0 ? 1 : weeks,
                Days = days,
                MaxEditableWeek = maxEditableWeek == 0 ? 1 : maxEditableWeek,
                EstimatedDueDate = estimatedDueDate,
                CalculationMethod = "LMP"
            };
        }

        public async Task<CreatePregnancyRecordResponse> CreatePregnancyRecord(CreatePregnancyRecordRequest request)
        {
            var response = new CreatePregnancyRecordResponse { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.BabyName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.BabyName),
                    Value = request.BabyName,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (!ValidationUtils.IsValidPregnancyStartDate(request.PregnancyStartDate, out var errorMessage))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.PregnancyStartDate),
                    Value = request.PregnancyStartDate.ToString(),
                    MessageId = Messages.E00002,
                    Message = errorMessage
                });
            }

            if (!ValidationUtils.IsValidExpectedDueDate(request.ExpectedDueDate, out errorMessage))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.ExpectedDueDate),
                    Value = request.ExpectedDueDate.ToString(),
                    MessageId = Messages.E00002,
                    Message = errorMessage
                });
            }

            if (!ValidationUtils.IsValidPregnancyDates(request.PregnancyStartDate, request.ExpectedDueDate, out errorMessage))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.ExpectedDueDate),
                    Value = request.ExpectedDueDate.ToString(),
                    MessageId = Messages.E00002,
                    Message = errorMessage
                });
            }

            var motherInfo = await _motherInfoRepository.GetByIdAsync(request.MotherInfoId);
            if (motherInfo == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.MotherInfoId),
                    Value = request.MotherInfoId.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00010;
                response.Message = Messages.GetMessageById(Messages.E00010);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var pregnancyRecord = new PregnancyRecord
            {
                Id = Guid.NewGuid(),
                MotherInfoId = request.MotherInfoId,
                BabyName = request.BabyName,
                PregnancyStartDate = request.PregnancyStartDate,
                ExpectedDueDate = request.ExpectedDueDate,
                BabyGender = request.BabyGender ?? string.Empty,
                ImageUrl = request.ImageUrl ?? string.Empty,
                MotherInfo = motherInfo
            };

            await _repository.AddAsync(pregnancyRecord);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<bool> DeletePregnancyRecord(Guid pregnancyRecordId)
        {
            var pregnancyRecord = (await _repository.FindAsync(x => x.Id == pregnancyRecordId && x.IsDeleted == false)).FirstOrDefault();
            if (pregnancyRecord == null) return false;

            pregnancyRecord.IsDeleted = true;
            _repository.Update(pregnancyRecord);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<PregnancyRecord>> GetAllPregnancyRecords(Guid motherInfoId)
        {
            return (await _repository.FindWithIncludesAsync(
                x => x.MotherInfoId == motherInfoId && x.IsDeleted == false,
                x => x.MotherInfo
            )).ToList();
        }

        public async Task<PregnancyRecord> GetPregnancyRecordById(Guid pregnancyRecordId)
        {
            return (await _repository.FindAsync(
                x => x.Id == pregnancyRecordId && x.IsDeleted == false
            )).FirstOrDefault();
        }

        public async Task<UpdatePregnancyRecordResponse> UpdatePregnancyRecord(Guid pregnancyRecordId, UpdatePregnancyRecordRequest request)
        {
            var response = new UpdatePregnancyRecordResponse { Success = false };
            var detailErrorList = new List<DetailError>();

            if (request.PregnancyStartDate.HasValue)
            {
                if (!ValidationUtils.IsValidPregnancyStartDate(request.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now), out var errorMessage))
                {
                    detailErrorList.Add(new DetailError
                    {
                        FieldName = nameof(request.PregnancyStartDate),
                        Value = request.PregnancyStartDate.ToString(),
                        MessageId = Messages.E00002,
                        Message = errorMessage
                    });
                }

                if (!ValidationUtils.IsValidExpectedDueDate(request.ExpectedDueDate, out errorMessage))
                {
                    detailErrorList.Add(new DetailError
                    {
                        FieldName = nameof(request.ExpectedDueDate),
                        Value = request.ExpectedDueDate.ToString(),
                        MessageId = Messages.E00002,
                        Message = errorMessage
                    });
                }

                if (!ValidationUtils.IsValidPregnancyDates(request.PregnancyStartDate ?? DateOnly.FromDateTime(DateTime.Now), request.ExpectedDueDate, out errorMessage))
                {
                    detailErrorList.Add(new DetailError
                    {
                        FieldName = nameof(request.ExpectedDueDate),
                        Value = request.ExpectedDueDate.ToString(),
                        MessageId = Messages.E00002,
                        Message = errorMessage
                    });
                }
            }

            if (detailErrorList.Any())
            {
                response.Success = false;
                response.MessageId = Messages.E00010;
                response.Message = Messages.GetMessageById(Messages.E00010);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var entity = (await _repository.FindWithIncludesAsync(x => x.Id == pregnancyRecordId && x.IsDeleted == false,
                x => x.MotherInfo
            )).FirstOrDefault();

            if (entity == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            entity.BabyName = request.BabyName ?? string.Empty;
            entity.BabyGender = request.BabyGender ?? string.Empty;
            entity.PregnancyStartDate = request.PregnancyStartDate;
            entity.ExpectedDueDate = request.ExpectedDueDate;
            entity.ImageUrl = request.ImageUrl ?? string.Empty;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
