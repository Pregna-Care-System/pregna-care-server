namespace PregnaCare.Common.Api
{
    /// <summary>
    /// AbstractApiResponse
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public abstract class AbstractApiResponse<U> where U : class
    {
        public string? MessageId { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; } = false;
        public abstract U Response { get; set; }
        public List<DetailError>? DetailErrorList { get; set; }
    }
}
