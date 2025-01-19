using System.Security.Cryptography;
using System.Text;

namespace PregnaCare.Utils
{
    public class PasswordUtils
    {
        /// <summary>
        /// HashPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// VerifyPassword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string hash)
        {
            return hash == HashPassword(password);  
        }
    }
}
