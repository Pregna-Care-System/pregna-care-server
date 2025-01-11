namespace PregnaCare.Core.Models
{
    /// <summary>
    /// Token
    /// </summary>
    public class Token
    {
        public string? RefreshToken { get; set; }    
        public string? AccessToken { get; set; }
    }
}
