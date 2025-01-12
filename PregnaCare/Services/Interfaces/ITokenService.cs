using Microsoft.AspNetCore.Identity;

namespace PregnaCare.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser identityUser, string roleName, string tokenType);
    }
}
