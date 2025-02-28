using PregnaCare.Api.Models.Requests.BlogRequestModel;
using PregnaCare.Api.Models.Requests.CommentBlogRequestModel;
using PregnaCare.Api.Models.Requests.FeatureRequestModel;
using PregnaCare.Api.Models.Requests.TagRequestModel;
using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Common.Mappers
{
    public static class Mapper
    {
        public static AccountDTO MapToAccountDTO(User user)
        {
            return new AccountDTO
            {
                Id = user.Id,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FullName = user.FullName,
                Gender = user.Gender,
                ImageUrl = user.ImageUrl,
                IsDeleted = user.IsDeleted,
                PhoneNumber = user.PhoneNumber,
            };
        }
        public static MembershipPlan MapToMembershipPlan(MembershipPlanRequest request)
        {
            return new MembershipPlan
            {
                PlanName = request.PlanName,
                Price = request.Price,
                Duration = request.Duration,
                Description = request.Description,
                ImageUrl = request.ImageUrl
            };
        }
        public static Feature MapToFeature(FeatureRequest request)
        {
            return new Feature
            {
                FeatureName = request.FeatureName,
                Description = request.Description,
            };
        }
        public static Tag MapToTag(TagRequest request)
        {
            return new Tag
            {
                Description = request.Description,
                Name = request.Name,
            };
        }
        public static Blog MapToBlog(BlogRequest blogRequest)
        {
            return new Blog
            {
                Content = blogRequest.Content,
                FeaturedImageUrl = blogRequest.FeaturedImageUrl,
                Heading = blogRequest.Heading, 
                PageTitle = blogRequest.PageTitle,
                ShortDescription = blogRequest.ShortDescription,
                UrlHandle = blogRequest.UrlHandle,
                UserId = blogRequest.UserId,
            };
        }
        public static Comment MapToComment(CreateCommentRequest request)
        {
            return new Comment
            {
                CommentText = request.CommentText,
                UserId = request.UserId,
                BlogId = request.BlogId,
                ParentCommentId = request.ParentCommentId,
            };
        }
        public static MembershipPlanFeatureDTO MapToMembershipPlanDTO(MembershipPlan plan)
        {
            return new MembershipPlanFeatureDTO
            {
                MembershipPlanId = plan.Id,
                PlanName = plan.PlanName,
                Price = plan.Price,
                Duration = plan.Duration,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                Features = plan.MembershipPlanFeatures.Select(x => new FeatureDTO
                {
                    FeatureName = x.Feature.FeatureName,
                    FeatureDescription = x.Feature.Description
                }).ToList()
            };
        }
    }
}
