﻿namespace PregnaCare.Core.DTOs
{
    public class BlogDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
        public string PageTitle { get; set; } = string.Empty;
        public string Heading { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public string FeaturedImageUrl { get; set; } = string.Empty;
        public bool? IsVisible { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? SharedChartData { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
    }
}
