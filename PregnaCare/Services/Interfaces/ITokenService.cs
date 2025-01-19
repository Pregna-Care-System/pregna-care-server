using Microsoft.AspNetCore.Identity;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, string roleName, string tokenType);
    }
}
