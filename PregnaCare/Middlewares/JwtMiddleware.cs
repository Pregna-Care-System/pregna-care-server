using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace PregnaCare.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var issuer = Environment.GetEnvironmentVariable("ISSUER");
                    var audience = Environment.GetEnvironmentVariable("AUDIENCE");
                    var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    httpContext.User = principal;
                }
                catch (SecurityTokenExpiredException)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Token has expired");
                }
                catch (Exception ex)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Invalid token");
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
