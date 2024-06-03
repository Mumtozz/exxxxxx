using Domain.Constants;
using Domain.DTOs.RolePermissionDTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class Seeder(DataContext context, IHashService hashService)
{
    public async Task Initial()
    {
        await SeedRole();
        await DefaultUsers();
    }



    private async Task SeedRole()
    {
        try
        {
            var newRoles = new List<Role>()
            {
                new()
                {
                    Name = Roles.SuperAdmin,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.Admin,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.User,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
            };

            var existing = await context.Roles.ToListAsync();
            foreach (var role in newRoles)
            {
                if (existing.Exists(e => e.Name == role.Name) == false)
                {
                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
        }
    }


    private async Task DefaultUsers()
    {
        try
        {
            var existingSuperAdmin = await context.Users.FirstOrDefaultAsync(x => x.Name == "Maruf");
            if (existingSuperAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "Maruf@gmail.com",
                    Name = "Maruf",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("221133")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Name == "Maruf");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdatedAt = DateTimeOffset.UtcNow,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await SeedClaimsForSuperAdmin();
            }



            var existingAdmin = await context.Users.FirstOrDefaultAsync(x => x.Name == "Admin");
            if (existingAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "admin@gmail.com",
                    Name = "Admin",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("12345")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Name == "Admin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdatedAt = DateTimeOffset.UtcNow,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await AddAdminPermissions();
            }



            var user = await context.Users.FirstOrDefaultAsync(x => x.Name == "User");
            if (user is null)
            {
                var superAdmin = new User()
                {
                    Email = "user@gmail.com",
                    Name = "User",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Name == "User");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdatedAt = DateTimeOffset.UtcNow,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

                await AddUserPermissions();
            }
        }
        catch (Exception e)
        {
        }
    }



    private async Task SeedClaimsForSuperAdmin()
    {
        try
        {
            var superAdminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
            if (superAdminRole == null) return;
            var roleClaims = new List<RoleClaimsDto>();
            roleClaims.GetPermissions(typeof(Domain.Constants.Permissions));
            var existingClaims = await context.RoleClaims.Where(x => x.RoleId == superAdminRole.Id).ToListAsync();
            foreach (var claim in roleClaims)
            {
                if (existingClaims.Any(c => c.ClaimType == claim.Value) == false)
                    await context.AddPermissionClaim(superAdminRole, claim.Value);
            }
        }
        catch (Exception ex)
        {
        }
    }



    private async Task AddUserPermissions()
    {
        var userRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
        if (userRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Users.View),
            new("Permissions", Domain.Constants.Permissions.Roles.View),
            new("Permissions", Domain.Constants.Permissions.Meetings.View),
            new("Permissions", Domain.Constants.Permissions.Notifications.View),
            new("Permissions", Domain.Constants.Permissions.UserRoles.View),
        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == userRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(userRole, claim.Value);
            }
        }
    }



    private async Task AddAdminPermissions()
    {
        var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
        if (adminRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Users.View),
            new("Permissions", Domain.Constants.Permissions.Users.Create),
            new("Permissions",Domain.Constants.Permissions.Users.Edit),

            new("Permissions", Domain.Constants.Permissions.Roles.View),
            new("Permissions", Domain.Constants.Permissions.Roles.Create),
            new("Permissions",Domain.Constants.Permissions.Roles.Edit),

            new("Permissions", Domain.Constants.Permissions.UserRoles.View),
            new("Permissions", Domain.Constants.Permissions.UserRoles.Create),
            new("Permissions",Domain.Constants.Permissions.UserRoles.Edit),

            new("Permissions", Domain.Constants.Permissions.Meetings.View),
            new("Permissions", Domain.Constants.Permissions.Meetings.Create),
            new("Permissions",Domain.Constants.Permissions.Meetings.Edit),

            new("Permissions", Domain.Constants.Permissions.Notifications.View),
            new("Permissions", Domain.Constants.Permissions.Notifications.Create),
        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == adminRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(adminRole, claim.Value);
            }
        }
    }

}