using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PregnaCare.Infrastructure.Data
{
    public class PregnaCareAuthDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public PregnaCareAuthDbContext(DbContextOptions<PregnaCareAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                _ = entity.Property<DateTime>("ExpirationTime");
            });
        }
    }
}
