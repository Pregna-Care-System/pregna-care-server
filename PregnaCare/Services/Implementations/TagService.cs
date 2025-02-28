using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Requests.TagRequestModel;
using PregnaCare.Api.Models.Responses.BlogResponseModel;
using PregnaCare.Api.Models.Responses.TagResponseModel;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TagListResponse> GetAllTags()
        {
            var tags = await _tagRepository.GetAllActiveTagAsync();
            return new TagListResponse
            {
                Success = true,
                Response = tags
            };
        }

        public async Task<TagResponse> GetTagById(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return new TagResponse
            {
                Success = true,
                Response = tag
            };
        }

        public async Task<TagResponse> CreateTag(TagRequest request)
        {
            var response = new TagResponse();

            var tag = Mapper.MapToTag(request);
            tag.Id = Guid.NewGuid();
            tag.CreatedAt = DateTime.Now;
            tag.UpdatedAt = DateTime.Now;
            tag.IsDeleted = false;

            await _tagRepository.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();

            response.Response = tag;
            response.Success = true;
            response.Message = "Create new tag successfully";
            return response;
        }

        public async Task<TagResponse> UpdateTag(TagRequest request, Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);

            tag.Description = request.Description;
            tag.Name = request.Name;
            tag.UpdatedAt = DateTime.Now;

             _tagRepository.Update(tag);
            await _unitOfWork.SaveChangesAsync();
            return new TagResponse
            {
                Success = true,
                Response = tag
            };
        }

        public async Task DeleteTag(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            _tagRepository.Remove(tag);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
