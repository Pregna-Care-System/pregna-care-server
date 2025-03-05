using PregnaCare.Api.Models.Requests.CommentBlogRequestModel;
using PregnaCare.Api.Models.Responses.CommentResponseModel;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repo;
        private readonly IUnitOfWork _unit;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _repo = commentRepository;
            _unit = unitOfWork;
        }

        public async Task<CommentResponse> CreateComment(CreateCommentRequest request)
        {
            var comment = Mapper.MapToComment(request);
            comment.IsDeleted = false;
            comment.UpdatedAt = DateTime.UtcNow;
            comment.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(comment);
            _ = _unit.SaveChangesAsync();

            return new CommentResponse
            {
                Response = comment,
                Success = true
            };
        }

        public async Task DeleteComment(Guid id)
        {
            var comment = await _repo.GetByIdAsync(id);
            _repo.Remove(comment);
            await _unit.SaveChangesAsync();
        }

        public async Task<CommentListResponse> GetAllBlogComment(Guid blogId)
        {
            var comment = await _repo.GetAllActiveCommentAsync(blogId);
            return new CommentListResponse
            {
                Success = true,
                Response = comment
            };
        }

        public async Task<CommentResponse> GetCommentById(Guid id)
        {
            var comment = await _repo.GetByIdAsync(id);
            return new CommentResponse
            {
                Response = comment,
                Success = true
            };
        }

        public async Task<CommentResponse> UpdateComment(UpdateCommentRequest request, Guid id)
        {
            var comment = await _repo.GetByIdAsync(id);
            comment.CommentText = request.CommentText;
            comment.UpdatedAt = DateTime.UtcNow;

            _repo.Update(comment);
            await _unit.SaveChangesAsync();

            return new CommentResponse
            {
                Success = true,
                Response = comment
            };
        }
    }
}
