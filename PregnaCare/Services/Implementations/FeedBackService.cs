using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Requests.FeedBackRequestModel;
using PregnaCare.Api.Models.Responses.FeatureResponseModel;
using PregnaCare.Api.Models.Responses.FeedbackResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class FeedBackService : IFeedBackService
    {

        private readonly IFeedBackRepository _feedbackRepository;
        private readonly IUnitOfWork _unit;

        public FeedBackService(IUnitOfWork unitOfWork, IFeedBackRepository feedBackRepository)
        {
            _unit = unitOfWork;
            _feedbackRepository = feedBackRepository;
        }
        public async Task<FeedbackResponse> AddFeedbackAsync(FeedbackRequest request, Guid userId)
        {
            var response = new FeedbackResponse();
           
            var feedback = Mapper.MapToFeedBack(request);
            feedback.Id = Guid.NewGuid();
            feedback.UserId = userId;
            feedback.Content = request.Content;
            feedback.Rating = request.Rating;
            feedback.UpdatedAt = DateTime.Now;
            feedback.CreatedAt = DateTime.Now;
            feedback.IsDeleted = false;

            await _feedbackRepository.AddAsync(feedback);
            await _unit.SaveChangesAsync();

            response.Success = true;
            response.Message = "Feedback added successfully";
            return response;
        }

        public async Task Delete(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                _ = new FeatureResponse
                {
                    Success = false,
                    Message = "Feedback not found",
                    MessageId = "E00004",
                };
            }
            feedback.IsDeleted = true;
            _feedbackRepository.Update(feedback);
            await _unit.SaveChangesAsync();
        }

        public async Task<FeedbackResponseList> GetAllFeedbackAsync()
        {
            var feedback = await _feedbackRepository.GetActiveFeatureAsync();
            return new FeedbackResponseList
            {
                Success = true,
                Message = "Feedback list retrieved successfully",
                Response = feedback
            };
        }

        public async Task<FeedbackResponse> GetFeedbackByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedBackByIdAsync(id);
            return new FeedbackResponse
            {
                Success = true,
                Message = "Feedback retrieved successfully",
                Response = feedback
            };
        }
    }
}
