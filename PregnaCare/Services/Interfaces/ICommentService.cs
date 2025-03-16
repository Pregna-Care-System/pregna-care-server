
using PregnaCare.Api.Models.Requests.CommentBlogRequestModel;
using PregnaCare.Api.Models.Responses.CommentResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface ICommentService
    {
        Task<SelectCommentResponse> GetAllBlogComment(Guid blogId);
        Task<SelectCommentResponse> GetCommentById(Guid id);
        Task<CommentResponse> CreateComment(CreateCommentRequest request);
        Task<CommentResponse> UpdateComment(UpdateCommentRequest request, Guid id);
        Task DeleteComment(Guid id);
    }
}
