using PregnaCare.Api.Models.Requests.FAQCategoryRequestModel;
using PregnaCare.Api.Models.Responses.FAQCategoryResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class FAQCategoryService : IFAQCategoryService
    {
        private readonly IGenericRepository<FAQCategory, Guid> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public FAQCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = unitOfWork.GetRepository<FAQCategory, Guid>();
        }

        public async Task<CreateFAQCategoryResponse> CreateCategoryAsync(CreateFAQCategoryRequest request)
        {
            var response = new CreateFAQCategoryResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.CreateFAQCategoryEntity.Name))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQCategoryEntity.Name),
                    Value = request.CreateFAQCategoryEntity.Name,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.CreateFAQCategoryEntity.Description))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQCategoryEntity.Description),
                    Value = request.CreateFAQCategoryEntity.Description,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (request.CreateFAQCategoryEntity.DisplayOrder == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQCategoryEntity.DisplayOrder),
                    Value = request.CreateFAQCategoryEntity.DisplayOrder.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var faqCategory = new FAQCategory
            {
                Name = request.CreateFAQCategoryEntity.Name,
                Description = request.CreateFAQCategoryEntity.Description,
                DisplayOrder = request.CreateFAQCategoryEntity.DisplayOrder,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _categoryRepository.AddAsync(faqCategory);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<DeleteFAQCategoryResponse> DeleteCategoryAsync(Guid id)
        {
            var response = new DeleteFAQCategoryResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var faqCategory = await _categoryRepository.GetByIdAsync(id);
            if (faqCategory == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            faqCategory.IsDeleted = true;
            faqCategory.UpdatedAt = DateTime.Now;

            _categoryRepository.Update(faqCategory);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<SelectFAQCategoryResponse> GetCategoriesAsync(string? name)
        {
            var response = new SelectFAQCategoryResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var faqCategories = await _categoryRepository.FindWithIncludesAsync(
                x => (string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower()))
                     && x.IsDeleted == false,
                x => x.FAQs);
            if (faqCategories.Count() == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var responseEntities = faqCategories.Where(x => x.IsDeleted == false).Select(x => new SelectFAQCategoryEntity
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                Items = x.FAQs.Where(x => x.IsDeleted == false).Select(y => new SelectFAQEntity
                {
                    Id = y.Id,
                    Question = y.Question,
                    Answer = y.Answer,
                    DisplayOrder = y.DisplayOrder,
                }).OrderBy(x => x.DisplayOrder).ToList()
            }).OrderBy(x => x.DisplayOrder).ToList();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
        }

        public async Task<SelectDetailFAQCategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            var response = new SelectDetailFAQCategoryResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var faqCategories = await _categoryRepository.FindWithIncludesAsync(x => x.Id == id && x.IsDeleted == false,
                x => x.FAQs);
            if (faqCategories.Count() == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var responseEntity = faqCategories.Select(x => new SelectFAQCategoryEntity
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                Items = x.FAQs.Select(y => new SelectFAQEntity
                {
                    Id = y.Id,
                    Question = y.Question,
                    Answer = y.Answer,
                    DisplayOrder = y.DisplayOrder,
                }).OrderBy(x => x.DisplayOrder).ToList()
            }).FirstOrDefault();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntity;
            return response;
        }

        public async Task<UpdateFAQCategoryResponse> UpdateCategoryAsync(Guid id, UpdateFAQCategoryRequest request)
        {
            var response = new UpdateFAQCategoryResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.UpdateFAQCategoryEntity.Name))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQCategoryEntity.Name),
                    Value = request.UpdateFAQCategoryEntity.Name,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.UpdateFAQCategoryEntity.Description))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQCategoryEntity.Description),
                    Value = request.UpdateFAQCategoryEntity.Description,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (request.UpdateFAQCategoryEntity.DisplayOrder == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQCategoryEntity.DisplayOrder),
                    Value = request.UpdateFAQCategoryEntity.DisplayOrder.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var faqCategory = (await _categoryRepository.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefault();
            if (faqCategory == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00002;
                response.Message = Messages.GetMessageById(Messages.E00002);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            faqCategory.Name = request.UpdateFAQCategoryEntity.Name;
            faqCategory.Description = request.UpdateFAQCategoryEntity.Description;
            faqCategory.DisplayOrder = request.UpdateFAQCategoryEntity.DisplayOrder;
            faqCategory.UpdatedAt = DateTime.Now;

            _categoryRepository.Update(faqCategory);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
