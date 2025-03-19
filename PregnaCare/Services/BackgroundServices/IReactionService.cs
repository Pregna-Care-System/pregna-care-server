using PregnaCare.Api.Models.Requests.ReactionRequestModel;
using PregnaCare.Api.Models.Responses.ReactionResponseModel;

namespace PregnaCare.Services.BackgroundServices
{
    public interface IReactionService
    {
        Task<SelectReactionResponse> SelectReactionByBlogIdAsync(Guid blogId);
        Task<SelectReactionResponse> SelectReactionByCommentIdAsync(Guid commentId);
        Task<CreateReactionResponse> CreateReactionAsync(CreateReactionRequest request);
        Task<UpdateReactionResponse> UpdateReactionAsync(Guid id, UpdateReactionRequest request);
        Task<DeleteReactionResponse> DeleteReactionAsync(Guid id);
    }
}
