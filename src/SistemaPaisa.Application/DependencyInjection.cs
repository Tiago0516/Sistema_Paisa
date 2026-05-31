using Microsoft.Extensions.DependencyInjection;
using SistemaPaisa.Application.Common.Behaviors;
using SistemaPaisa.Application.Common.Auth;
using SistemaPaisa.Application.Common.ModuleActions;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Common.ProfileModules;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Application.Common.Users;

namespace SistemaPaisa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(SupplierRequestLoggingBehavior<,>));
        });
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserRegistrationService, UserRegistrationService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IProfileRoleService, ProfileRoleService>();
        services.AddScoped<IProfileModuleService, ProfileModuleService>();
        services.AddScoped<IModuleActionService, ModuleActionService>();
        return services;
    }
}