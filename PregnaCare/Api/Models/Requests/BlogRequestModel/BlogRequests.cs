using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.BlogRequestModel
{
    public class BlogRequests : AbstractApiRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}