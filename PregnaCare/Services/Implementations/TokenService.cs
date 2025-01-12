﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using PregnaCare.Common.Enums;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class TokenService : ITokenService
    {

        /// <summary>
        /// GenerateTokenAsync
        /// </summary>
        /// <param name="identityUser"></param>
        /// <param name="roleName"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateToken(IdentityUser identityUser, string roleName, string tokenType)
        {
            var token = string.Empty;

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
                new Claim(ClaimTypes.Email, identityUser.Email),
                new Claim(ClaimTypes.Role, roleName),
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

            token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
