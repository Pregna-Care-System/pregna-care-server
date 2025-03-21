﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;

namespace PregnaCare.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var appDbContext = serviceProvider.GetRequiredService<PregnaCareAppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            using (var transaction = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Seed roles
                    var roleNames = new List<string> { RoleEnum.Admin.ToString(), RoleEnum.Member.ToString(), RoleEnum.Guest.ToString(), RoleEnum.User.ToString() };

                    // Tải tất cả các role từ database vào bộ nhớ
                    var existingRoles = await appDbContext.Roles
                        .AsNoTracking()
                        .Select(r => r.RoleName)
                        .ToListAsync();

                    var isAdded = false;
                    foreach (var roleName in roleNames)
                    {
                        if (!existingRoles.Contains(roleName))
                        {
                            _ = await appDbContext.Roles.AddAsync(new Role
                            {
                                RoleName = roleName,
                                Description = roleName,
                                IsDeleted = false,
                            });

                            _ = await roleManager.CreateAsync(new IdentityRole<Guid>
                            {
                                Id = Guid.NewGuid(),
                                Name = roleName,
                                NormalizedName = roleName,
                            });

                            isAdded = true;
                        }
                    }

                    if (isAdded) _ = await appDbContext.SaveChangesAsync();

                    // Seed admin account
                    var adminEmail = "pregnacareadmin8386@gmail.com";

                    if (!await appDbContext.Users.AnyAsync(u => u.Email == adminEmail))
                    {
                        var adminRole = await appDbContext.Roles
                            .FirstOrDefaultAsync(r => r.RoleName == RoleEnum.Admin.ToString());

                        if (adminRole == null)
                        {
                            throw new InvalidOperationException("Admin role not found. Please ensure roles are seeded correctly.");
                        }

                        var admin = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = adminEmail,
                            FullName = "PregnaCare Admin",
                            IsDeleted = false
                        };

                        var userRole = new UserRole
                        {
                            Id = Guid.NewGuid(),
                            UserId = admin.Id,
                            RoleId = adminRole.Id,
                        };

                        var identityUser = new IdentityUser<Guid>
                        {
                            Id = admin.Id,
                            UserName = adminEmail,
                            NormalizedUserName = adminEmail,
                            Email = adminEmail,
                            NormalizedEmail = adminEmail,
                        };

                        _ = await userManager.CreateAsync(identityUser, "Admin1234@!");
                        _ = await userManager.AddToRoleAsync(identityUser, adminRole.RoleName);

                        _ = await appDbContext.UserRoles.AddAsync(userRole);
                        _ = await appDbContext.Users.AddAsync(admin);
                        _ = await appDbContext.SaveChangesAsync();
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
