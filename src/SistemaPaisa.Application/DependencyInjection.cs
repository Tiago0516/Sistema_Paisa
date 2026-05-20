using Microsoft.Extensions.DependencyInjection;
using SistemaPaisa.Application.Common.ModuleActions;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Common.ProfileModules;

namespace SistemaPaisa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IModuleActionService, ModuleActionService>();
        services.AddScoped<IProfileModuleService, ProfileModuleService>();
        return services;
    }
}