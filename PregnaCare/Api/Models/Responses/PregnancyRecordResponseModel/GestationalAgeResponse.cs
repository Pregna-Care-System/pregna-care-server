namespace PregnaCare.Api.Models.Responses.PregnancyRecordResponseModel
{
    public class GestationalAgeResponse
    {
        public int Weeks { get; set; }
        public int Days { get; set; }
        public DateTime EstimatedDueDate { get; set; }
        public string CalculationMethod { get; set; }
    }
}
