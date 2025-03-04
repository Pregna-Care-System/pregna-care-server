using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.CommentBlogRequestModel
{
    public class CreateCommentRequest: AbstractApiRequest
    {
        public Guid BlogId { get; set; }

        public Guid UserId { get; set; }

        public Guid? ParentCommentId { get; set; }

        public string CommentText { get; set; } = string.Empty;

    }
}
