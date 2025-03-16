using System.Security.Cryptography;

namespace PregnaCare.Utils
{
    public class CommonUtils
    {
        public static string GenerateOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int otp = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
                return otp.ToString("D6");
            }
        }

        public static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);

            if (timeSpan.TotalSeconds < 60)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} {(timeSpan.TotalMinutes == 1 ? "minute" : "minutes")} ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} {(timeSpan.TotalHours == 1 ? "hour" : "hours")} ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} {(timeSpan.TotalDays == 1 ? "day" : "days")} ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} {((int)(timeSpan.TotalDays / 7) == 1 ? "week" : "weeks")} ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} {((int)(timeSpan.TotalDays / 30) == 1 ? "month" : "months")} ago";

            return $"{(int)(timeSpan.TotalDays / 365)} {((int)(timeSpan.TotalDays / 365) == 1 ? "year" : "years")} ago";
        }

    }
}
