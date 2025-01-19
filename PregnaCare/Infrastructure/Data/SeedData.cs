using Microsoft.EntityFrameworkCore;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Utils;

namespace PregnaCare.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var appDbContext = serviceProvider.GetRequiredService<PregnaCareAppDbContext>();

            using (var transaction = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Seed roles
                    var roleNames = new List<string> { RoleEnum.Admin.ToString(), RoleEnum.Member.ToString(), RoleEnum.Guest.ToString() };

                    // Tải tất cả các role từ database vào bộ nhớ
                    var existingRoles = await appDbContext.Roles
                        .AsNoTracking()
                        .Select(r => r.RoleName)
                        .ToListAsync();

                    foreach (var roleName in roleNames)
                    {
                        if (!existingRoles.Contains(roleName))
                        {
                            await appDbContext.Roles.AddAsync(new Role
                            {
                                RoleName = roleName,
                                Description = roleName,
                                IsDeleted = false,
                            });
                        }
                    }

                    await appDbContext.SaveChangesAsync();

                    // Seed admin account
                    var adminEmail = "pregnacareadmin@gmail.com";

                    if (!await appDbContext.Users.AnyAsync(u => u.Email == adminEmail))
                    {
                        var hashPassword = PasswordHasher.HashPassword("Admin1234@!");
                        var adminRole = await appDbContext.Roles
                            .FirstOrDefaultAsync(r => r.RoleName == RoleEnum.Admin.ToString());

                        if (adminRole == null)
                        {
                            throw new InvalidOperationException("Admin role not found. Please ensure roles are seeded correctly.");
                        }

                        var admin = new User
                        {
                            Email = adminEmail,
                            FullName = "PregnaCare Admin",
                            Password = hashPassword,
                            Role = adminRole,
                            RoleId = adminRole.Id,
                            IsDeleted = false
                        };

                        await appDbContext.Users.AddAsync(admin);
                        await appDbContext.SaveChangesAsync();
                    }

                    // Commit transaction
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback transaction
                    await transaction.RollbackAsync();

                    // Log lỗi
                    Console.WriteLine($"Error occurred while seeding data: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
