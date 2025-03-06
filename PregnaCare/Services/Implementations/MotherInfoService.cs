using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Requests.MotherInfoModel;
using PregnaCare.Api.Models.Requests.MotherInfoRequestModel;
using PregnaCare.Api.Models.Responses.MotherInfoResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class MotherInfoService : IMotherInfoService
    {
        private readonly PregnaCareAppDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public MotherInfoService(PregnaCareAppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateMotherInfoResponse> CreateMotherInfoAsync(CreateMotherInfoRequest request)
        {
            var response = new CreateMotherInfoResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.MotherName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.MotherName),
                    Value = request.MotherName,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.BloodType))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.BloodType),
                    Value = request.BloodType,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UserId),
                    Value = request.UserId.ToString(),
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

            var motherInfo = new MotherInfo
            {
                Id = Guid.NewGuid(),
                MotherName = request.MotherName,
                BloodType = request.BloodType,
                HealthStatus = request.HealhStatus,
                Notes = request.Notes,
                UserId = request.UserId,
                DateOfBirth = request.MotherDateOfBirth
            };

            _ = await _context.MotherInfos.AddAsync(motherInfo);
            _ = await _context.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public IEnumerable<MotherInfo> GetAllMotherInfosByUserId(Guid userId)
        {
            return _context.MotherInfos.AsNoTracking().Where(x => x.UserId == userId);
        }

        public async Task<UpdateMotherInfoResponse> UpdateMotherInfoAsync(Guid motherInfoId, UpdateMotherInfoRequest request)
        {
            var response = new UpdateMotherInfoResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.MotherName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.MotherName),
                    Value = request.MotherName,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.BloodType))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.BloodType),
                    Value = request.BloodType,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var motherInfo = await _context.MotherInfos.FirstOrDefaultAsync(x => x.Id == motherInfoId);
            if (motherInfo == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(motherInfoId),
                    Value = motherInfoId.ToString(),
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

            motherInfo.MotherName = request.MotherName;
            motherInfo.DateOfBirth = request.MotherDateOfBirth;
            motherInfo.BloodType = request.BloodType;
            motherInfo.HealthStatus = request.HealhStatus;
            motherInfo.Notes = request.Notes;

            _ = await _context.MotherInfos.AddAsync(motherInfo);
            _ = await _context.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
