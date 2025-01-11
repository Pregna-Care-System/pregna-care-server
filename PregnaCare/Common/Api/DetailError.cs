namespace PregnaCare.Common.Api
{
    /// <summary>
    /// DetailError
    /// </summary>
    public class DetailError
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string ColName { get; set; }
        public int RowNumber { get; set; }
    }
}
