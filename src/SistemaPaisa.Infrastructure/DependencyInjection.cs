using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;
using SistemaPaisa.Infrastructure.Repositories;
using SistemaPaisa.Infrastructure.Security;

namespace SistemaPaisa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IActionRepository, ActionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IModuleActionRepository, ModuleActionRepository>();
        services.AddScoped<IProfileModuleRepository, ProfileModuleRepository>();
        services.AddScoped<IProfileRoleRepository, ProfileRoleRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        return services;
    }
}
