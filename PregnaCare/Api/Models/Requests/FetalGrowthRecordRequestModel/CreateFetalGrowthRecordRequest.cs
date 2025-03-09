using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FetalGrowthRecordRequestModel
{
    public class CreateFetalGrowthRecordRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }

        public Guid PregnancyRecordId { get; set; }

        public int? Week { get; set; }


        public List<CreateFetalGrowthRecordEntity> CreateFetalGrowthRecordEntities { get; set; }
    }

    public class CreateFetalGrowthRecordEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double? Value { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
