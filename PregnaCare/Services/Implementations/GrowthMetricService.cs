using PregnaCare.Api.Models.Requests.GrowthMetricRequestModel;
using PregnaCare.Api.Models.Responses.GrowthMetricResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class GrowthMetricService : IGrowthMetricService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<GrowthMetric, Guid> _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public GrowthMetricService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<GrowthMetric, Guid>();
        }

        public async Task<CreateGrowthMetricResponse> CreateGrowthMetric(CreateGrowthMetricRequest request)
        {
            var response = new CreateGrowthMetricResponse { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.Name))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Name),
                    Value = request.Name,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.Unit))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Unit),
                    Value = request.Unit,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Name),
                    Value = request.Name,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Any())
            {
                response.MessageId = Messages.E00010;
                response.Message = Messages.GetMessageById(Messages.E00010);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var growthMetric = new GrowthMetric
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Unit = request.Unit,
                Description = request.Description,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                Week = request.Week,
            };

            await _repository.AddAsync(growthMetric);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<bool> DeleteGrowthMetric(Guid id)
        {
            var entity = (await _repository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();
            if (entity == null) return false;

            entity.IsDeleted = true;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<GrowthMetric>> GetAllGrowthMetrics()
        {
            return (await _repository.GetAllAsync()).ToList();
        }

        public async Task<List<GrowthMetric>> GetAllGrowthMetricsByWeek(int week)
        {
           return (await _repository.FindAsync(x => x.Week == week && x.IsDeleted == false)).ToList();
        }

        public async Task<GrowthMetric> GetGrowthMetricById(Guid id)
        {
            return (await _repository.FindAsync(x => x.Id == id &&
                                                     x.IsDeleted == false))
                         .FirstOrDefault();
        }

        public async Task<UpdateGrowthMetricResponse> UpdateGrowthMetric(UpdateGrowthMetricRequest request)
        {
            var response = new UpdateGrowthMetricResponse { Success = false };

            var growthMetric = (await _repository.FindAsync(x => x.Id == request.Id && x.IsDeleted == false)).FirstOrDefault();
            if (growthMetric == null)
            {
                response.MessageId = Messages.E00000;
                response.Message = Messages.GetMessageById(Messages.E00000);
                return response;
            }

            growthMetric.Name = request.Name;
            growthMetric.Unit = request.Unit;
            growthMetric.Description = request.Description;
            growthMetric.Week = request.Week;
            growthMetric.MinValue = request.MinValue;
            growthMetric.MaxValue = request.MaxValue;
            growthMetric.Week = request.Week;

            _repository.Update(growthMetric);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
