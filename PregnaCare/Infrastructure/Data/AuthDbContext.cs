using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PregnaCare.Infrastructure.Data
{
    /// <summary>
    /// AuthDbContext
    /// </summary>
    public class AuthDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthDbContext() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    }
}
