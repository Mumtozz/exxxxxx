using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.NotificationService;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Infrastructure.Services.RoleService;
using Infrastructure.Services.UserRoleService;
using Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services.MeetingService;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Permissions;

namespace WebApp.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}