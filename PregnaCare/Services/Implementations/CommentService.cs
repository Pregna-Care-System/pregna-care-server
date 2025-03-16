using Azure;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Api.Models.Requests.CommentBlogRequestModel;
using PregnaCare.Api.Models.Responses.CommentResponseModel;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Mappers;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

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

        public async Task<CommentResponse> CreateComment([FromBody] CreateCommentRequest request)
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

        public async Task<SelectCommentResponse> GetAllBlogComment(Guid blogId)
        {
            var response = new SelectCommentResponse() { Success = false };

            var allComments = await _repo.GetAllActiveCommentAsync(blogId);
            var parentComments = allComments.Where(c => c.ParentCommentId == null).ToList();
            var commentDictionary = allComments.ToDictionary(c => c.Id);
            var responseEntities = parentComments.Select(x => MapCommentWithReplies(x, commentDictionary, 0, 2)).ToList();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
        }

        public async Task<SelectCommentResponse> GetCommentById(Guid id)
        {
            var response = new SelectCommentResponse() { Success = false };

            var comments = (await _repo.FindWithIncludesAsync(x => x.Id == id && x.IsDeleted == false, x => x.User)).ToList();
            var commentDictionary = comments.ToDictionary(c => c.Id);
            var responseEntities = comments.Select(x => MapCommentWithReplies(x, commentDictionary, 0, 2)).ToList();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
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

        private SelectCommentEntity MapCommentWithReplies(Comment comment, Dictionary<Guid, Comment> allComments, int currentDepth, int maxDepth)
        {
            var commentEntity = new SelectCommentEntity
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                User = new UserBriefInfo
                {
                    Id = comment.User.Id,
                    FullName = comment.User.FullName,
                    AvatarUrl = comment.User.ImageUrl,
                },
                CreatedAt = comment?.CreatedAt ?? DateTime.Now,
                UpdatedAt = comment?.UpdatedAt ?? DateTime.Now,
                BlogId = comment.BlogId,
                TimeAgo = CommonUtils.GetTimeAgo(comment.UpdatedAt.Value),
                Replies = new List<SelectCommentEntity>()
            };

            if (currentDepth < maxDepth)
            {
                var childComments = comment.InverseParentComment
                    .Where(c => c.IsDeleted != true)
                    .OrderByDescending(c => c.UpdatedAt)
                    .ThenByDescending(c => c.CreatedAt)
                    .ToList();

                foreach (var childComment in childComments)
                {
                    var childEntity = MapCommentWithReplies(childComment, allComments, currentDepth + 1, maxDepth);
                    commentEntity.Replies.Add(childEntity);
                }
            }

            return commentEntity;
        }
    }
}
