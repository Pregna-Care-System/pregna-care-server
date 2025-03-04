
using PregnaCare.Api.Models.Requests.TagRequestModel;
using PregnaCare.Api.Models.Responses.TagResponseModel;

namespace PregnaCare.Services.Interfaces

{
    public interface ITagService
    {
        Task<TagListResponse> GetAllTags();
        Task<TagResponse> GetTagById(Guid id);
        Task<TagResponse> CreateTag(TagRequest request);
        Task<TagResponse> UpdateTag(TagRequest request, Guid id);
        Task DeleteTag(Guid id);
    }
}
