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
    }
}
