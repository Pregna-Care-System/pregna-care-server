using Microsoft.AspNetCore.Identity;
using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterAsync(User userAccount, IdentityUser identityUser, string password, string roleName);
        Task LoginAsync();
    }
}
