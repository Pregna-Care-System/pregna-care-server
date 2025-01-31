using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _repo;
        private readonly IUnitOfWork _unit;
        public FeatureService(IFeatureRepository featureRepository, IUnitOfWork unitOfWork)
        {
            _repo = featureRepository;
            _unit = unitOfWork;
        }
        public async Task<FeatureResponse> AddFeatureAsync(FeatureRequest request)
        {
            var response = new FeatureResponse();
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.FeatureName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.FeatureName),
                    Value = request.FeatureName,
                    Message = "Feature name is required",
                    MessageId = "E00005"
                });
            }
            var feature = Mapper.MapToFeature(request);
            feature.Id = Guid.NewGuid();
            feature.FeatureName = request.FeatureName;
            feature.Description = request.Description;
            feature.UpdatedAt = DateTime.Now;
            feature.CreatedAt = DateTime.Now;
            feature.IsDeleted = false;

            await _repo.AddAsync(feature);
            await _unit.SaveChangesAsync();

            response.Success = true;
            response.Message = "Feature added successfully";
            return response;
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Feature> GetFeatureById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, Feature feature)
        {
            throw new NotImplementedException();
        }
        
    }
}
