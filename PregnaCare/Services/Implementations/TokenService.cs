using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class TokenService : ITokenService
    {

        /// <summary>
        /// GenerateTokenAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateToken(User user, string roleName, string tokenType)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var audience = Environment.GetEnvironmentVariable("AUDIENCE");
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("Issuer, Audience, or SecretKey is not provided.");
            }

            var expiration = DateTime.Now;

            if (tokenType == TokenTypeEnum.AccessToken.ToString())
            {
                var refreshTokenExpiration = Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRATION") ?? "0";
                expiration = expiration.AddDays(double.Parse(refreshTokenExpiration));
            }
            else
            {
                var accessTokenExpiration = Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRATION") ?? "0";
                expiration = expiration.AddMinutes(double.Parse(accessTokenExpiration));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", roleName),
                new Claim("name", user.FullName),
                new Claim("picture", user.ImageUrl),
                new Claim("address", user.Address),
                new Claim("phone", user.PhoneNumber),
                new Claim("gender", user.Gender),
                new Claim("dateOfBirth", user.DateOfBirth?.ToString("dd-MM-yyyy") ?? DateOnly.FromDateTime(DateTime.Now).ToString("dd-MM-yyyy"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            string? token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
