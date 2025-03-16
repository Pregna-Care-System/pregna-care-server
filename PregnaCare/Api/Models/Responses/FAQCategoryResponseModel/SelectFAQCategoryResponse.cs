using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQCategoryResponseModel
{
    public class SelectFAQCategoryResponse : AbstractApiResponse<List<SelectFAQCategoryEntity>>
    {
        public override List<SelectFAQCategoryEntity> Response { get; set; }
    }

    public class SelectFAQCategoryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public List<SelectFAQEntity> Items { get; set; }
    }

    public class SelectFAQEntity
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
    }
}
