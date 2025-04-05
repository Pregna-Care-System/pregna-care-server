using PregnaCare.Api.Models.Requests.ReactionRequestModel;
using PregnaCare.Api.Models.Responses.ReactionResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.BackgroundServices;

namespace PregnaCare.Services.Implementations
{
    public class ReactionService : IReactionService
    {
        private readonly IGenericRepository<Reaction, Guid> _reactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ReactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _reactionRepository = _unitOfWork.GetRepository<Reaction, Guid>();
        }

        public async Task<CreateReactionResponse> CreateReactionAsync(CreateReactionRequest request)
        {
            var response = new CreateReactionResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var user = await _unitOfWork.GetRepository<User, Guid>().GetByIdAsync(request.UserId);
            if (user == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.UserId),
                    Value = request.UserId.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (Enum.GetValues(typeof(ReactionEnum)).Cast<ReactionEnum>().All(x => ((int)x).ToString() != request.Type))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Type),
                    Value = request.Type,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00001;
                response.Message = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            Blog? blog = null;
            if (request.BlogId.HasValue)
            {
                blog = await _unitOfWork.GetRepository<Blog, Guid>().GetByIdAsync(request.BlogId.Value);
            }

            Comment? comment = null;
            if (request.CommentId.HasValue)
            {
                comment = await _unitOfWork.GetRepository<Comment, Guid>().GetByIdAsync(request.CommentId.Value);
            }

            var reaction = new Reaction
            {
                UserId = request.UserId,
                BlogId = blog?.Id ?? null,
                CommentId = comment?.Id ?? null,
                Type = request.Type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _reactionRepository.AddAsync(reaction);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<DeleteReactionResponse> DeleteReactionAsync(Guid id)
        {
            var response = new DeleteReactionResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var reaction = await _reactionRepository.GetByIdAsync(id);
            if (reaction == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00001;
                response.Message = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            _reactionRepository.Remove(reaction);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<SelectReactionResponse> SelectReactionByBlogIdAsync(Guid blogId)
        {
            var response = new SelectReactionResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var blog = await _unitOfWork.GetRepository<Blog, Guid>().GetByIdAsync(blogId);
            if (blog == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(blogId),
                    Value = blogId.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00001;
                response.Message = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var reactions = await _reactionRepository.FindWithIncludesAsync(x => x.BlogId == blogId, x => x.Blog, x => x.User);

            var responseEntities = reactions.OrderByDescending(x => x.UpdatedAt)
                                          .ThenByDescending(x => x.CreatedAt)
                                          .Select(x => new SelectReactionEntity
                                          {
                                              Id = x.Id,
                                              UserId = x.UserId,
                                              FullName = x.User.FullName,
                                              UserAvatarUrl = x.User.ImageUrl,
                                              Type = x.Type,
                                          }).ToList();

            if (responseEntities.Count == 0)
            {
                response.MessageId = Messages.E00013;
                response.Message = Messages.GetMessageById(Messages.E00013);
                return response;
            }

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
        }

        public async Task<SelectReactionResponse> SelectReactionByCommentIdAsync(Guid commentId)
        {
            var response = new SelectReactionResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            var reactions = await _reactionRepository.FindWithIncludesAsync(x => x.CommentId == commentId, x => x.Comment, x => x.User);

            var responseEntities = reactions.OrderByDescending(x => x.UpdatedAt)
                                         .ThenByDescending(x => x.CreatedAt)
                                         .Select(x => new SelectReactionEntity
                                         {
                                             Id = x.Id,
                                             UserId = x.UserId,
                                             FullName = x.User.FullName,
                                             UserAvatarUrl = x.User.ImageUrl,
                                             Type = x.Type,
                                         }).ToList();

            if (responseEntities.Count == 0)
            {
                response.MessageId = Messages.E00013;
                response.Message = Messages.GetMessageById(Messages.E00013);
                return response;
            }

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
        }

        public async Task<UpdateReactionResponse> UpdateReactionAsync(Guid id, UpdateReactionRequest request)
        {
            var response = new UpdateReactionResponse() { Success = false };
            var detailErrorList = new List<DetailError>();

            if (Enum.GetValues(typeof(ReactionEnum)).Cast<ReactionEnum>().All(x => ((int)x).ToString() != request.Type))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Type),
                    Value = request.Type,
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            var reaction = await _reactionRepository.GetByIdAsync(id);
            if (reaction == null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(id),
                    Value = id.ToString(),
                    MessageId = Messages.E00002,
                    Message = Messages.GetMessageById(Messages.E00002)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00001;
                response.Message = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            reaction.Type = request.Type;
            reaction.UpdatedAt = DateTime.Now;

            _reactionRepository.Update(reaction);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            return response;
        }
    }
}
