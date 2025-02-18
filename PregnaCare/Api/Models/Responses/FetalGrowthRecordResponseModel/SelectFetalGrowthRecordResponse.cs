namespace PregnaCare.Api.Models.Responses.FetalGrowthRecordResponseModel
{
    public class SelectFetalGrowthRecordResponse
    {
        public Guid Id { get; set; }

        public Guid PregnancyRecordId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? Week { get; set; }

        public double? Value { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
