using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PregnaCareAppDbContext _pregnaCareAppDbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="pregnaCareAppDbContext"></param>
        public AuthRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, PregnaCareAppDbContext pregnaCareAppDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _pregnaCareAppDbContext = pregnaCareAppDbContext;
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public async Task RegisterAsync(UserAccount userAccount, IdentityUser identityUser, string password, string roleName)
        {
            await _userManager.AddPasswordAsync(identityUser, password);
            await _userManager.CreateAsync(identityUser);
            await _userManager.AddToRoleAsync(identityUser, roleName);
            await _pregnaCareAppDbContext.UserAccounts.AddAsync(userAccount);

            await _pregnaCareAppDbContext.SaveChangesAsync();
        }
    }
}
