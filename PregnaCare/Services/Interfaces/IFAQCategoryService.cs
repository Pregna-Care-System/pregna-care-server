using PregnaCare.Api.Models.Requests.FAQCategoryRequestModel;
using PregnaCare.Api.Models.Responses.FAQCategoryResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IFAQCategoryService
    {
        Task<SelectFAQCategoryResponse> GetCategoriesAsync(string? name);
        Task<SelectDetailFAQCategoryResponse> GetCategoryByIdAsync(Guid id);
        Task<CreateFAQCategoryResponse> CreateCategoryAsync(CreateFAQCategoryRequest request);
        Task<UpdateFAQCategoryResponse> UpdateCategoryAsync(Guid id, UpdateFAQCategoryRequest request);
        Task<DeleteFAQCategoryResponse> DeleteCategoryAsync(Guid id);
    }
}
