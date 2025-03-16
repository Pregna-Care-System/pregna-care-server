using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.CommentResponseModel
{
    public class SelectCommentResponse : AbstractApiResponse<List<SelectCommentEntity>>
    {
        public override List<SelectCommentEntity> Response { get; set; }
    }

    public class SelectCommentEntity
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public UserBriefInfo User { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<SelectCommentEntity> Replies { get; set; } = new List<SelectCommentEntity>();
        public string TimeAgo { get; set; }
    }

    public class UserBriefInfo
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
