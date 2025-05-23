﻿using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
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

        public async Task<FeatureResponse> Delete(Guid id)
        {
            var feature = await _repo.GetByIdAsync(id);
            if (feature == null)
            {
                _ = new FeatureResponse
                {
                    Success = false,
                    Message = "Feature not found",
                    MessageId = "E00004",
                };
            }

            feature.IsDeleted = true;
            _repo.Update(feature);
            await _unit.SaveChangesAsync();

            return new FeatureResponse
            {
                Success = true,
                Message = "Delete feature successfully",
            };
        }

        public async Task<FeatureListResponse> GetAllFeaturesAsync()
        {
            var feature = await _repo.GetActiveFeatureAsync();
            return new FeatureListResponse
            {
                Success = true,
                Message = "Feature list retrieved successfully",
                Response = feature
            };
        }

        public async Task<List<SelectFeatureResponse>> GetAllFeaturesByUserIdAsync(Guid userId)
        {
            var responseEntity = await _repo.GetAllFeaturesByUserId(userId);
            if (!responseEntity.Any()) return null;

            return responseEntity
                .OrderBy(x => x.FeatureName)
                .ThenBy(x => x.CreatedAt)
                .Select(x => new SelectFeatureResponse
                {
                    FeatureId = x.Id,
                    FeatureName = x.FeatureName,
                }).ToList();
        }

        public async Task<FeatureResponse> GetFeatureById(Guid id)
        {
            var feature = await _repo.GetByIdAsync(id);
            return new FeatureResponse
            {
                Success = true,
                Response = feature
            };
        }

        public async Task<FeatureResponse> Update(Guid id, FeatureRequest request)
        {
            var feature = await _repo.GetByIdAsync(id);
            if (feature == null)
            {
                return new FeatureResponse
                {
                    Success = false,
                    Message = "Feature not found",
                    MessageId = "E00004",
                };
            }
            feature.FeatureName = request.FeatureName;
            feature.Description = request.Description;
            feature.UpdatedAt = DateTime.Now;

            _repo.Update(feature);
            await _unit.SaveChangesAsync();
            return new FeatureResponse
            {
                Success = true,
                Message = "Feature updated successfully",
                Response = feature
            };
        }

    }
}
