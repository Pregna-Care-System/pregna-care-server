using PregnaCare.Api.Models.Requests.FAQRequestModel;
using PregnaCare.Api.Models.Responses.FAQResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class FAQService : IFAQService
    {
        private readonly IGenericRepository<FAQ, Guid> _faqRepository;
        private readonly IGenericRepository<FAQCategory, Guid> _faqCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FAQService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _faqRepository = unitOfWork.GetRepository<FAQ, Guid>();
            _faqCategoryRepository = unitOfWork.GetRepository<FAQCategory, Guid>();
        }

        public async Task<CreateFAQResponse> CreateFAQAsync(CreateFAQRequest request)
        {
            var response = new CreateFAQResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.CreateFAQEntity.Question))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQEntity.Question),
                    Value = request.CreateFAQEntity.Question,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.CreateFAQEntity.Answer))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQEntity.Answer),
                    Value = request.CreateFAQEntity.Answer,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (request.CreateFAQEntity.DisplayOrder == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.CreateFAQEntity.DisplayOrder),
                    Value = request.CreateFAQEntity.DisplayOrder.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var faqCategory = await _faqCategoryRepository.GetByIdAsync(request.CreateFAQEntity.FAQCategoryId);
            if (faqCategory == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(faqCategory),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var faq = new FAQ
            {
                CategoryId = request.CreateFAQEntity.FAQCategoryId,
                Question = request.CreateFAQEntity.Question,
                Answer = request.CreateFAQEntity.Answer,
                DisplayOrder = request.CreateFAQEntity.DisplayOrder,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            faqCategory.UpdatedAt = DateTime.Now;

            await _faqRepository.AddAsync(faq);
            _faqCategoryRepository.Update(faqCategory);

            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<DeleteFAQResponse> DeleteFAQAsync(Guid id)
        {
            var response = new DeleteFAQResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(faq),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            faq.IsDeleted = true;
            faq.UpdatedAt = DateTime.Now;

            _faqRepository.Update(faq);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.MessageId = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<UpdateFAQResponse> UpdateFAQAsync(Guid id, UpdateFAQRequest request)
        {
            var response = new UpdateFAQResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.UpdateFAQEntity.Question))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQEntity.Question),
                    Value = request.UpdateFAQEntity.Question,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (string.IsNullOrEmpty(request.UpdateFAQEntity.Answer))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQEntity.Answer),
                    Value = request.UpdateFAQEntity.Answer,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (request.UpdateFAQEntity.DisplayOrder == 0)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UpdateFAQEntity.DisplayOrder),
                    Value = request.UpdateFAQEntity.DisplayOrder.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var faq = (await _faqRepository.FindWithIncludesAsync(x => x.Id == id && x.IsDeleted == false, x => x.Category)).FirstOrDefault();
            if (faq == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(faq),
                    MessageId = Messages.E00013,
                    Message = Messages.GetMessageById(Messages.E00013)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            faq.Question = request.UpdateFAQEntity.Question;
            faq.Answer = request.UpdateFAQEntity.Answer;
            faq.DisplayOrder = request.UpdateFAQEntity.DisplayOrder;
            faq.UpdatedAt = DateTime.Now;
            faq.Category.UpdatedAt = DateTime.Now;

            _faqRepository.Update(faq);
            _faqCategoryRepository.Update(faq.Category);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.MessageId = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
