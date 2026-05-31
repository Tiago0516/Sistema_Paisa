using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Data;

internal static class ModuleSeeder
{
    private const int SystemUserId = 1;
    private sealed record ModuleSeed(
        string Code,
        string Name,
        string Description,
        ActionSeed[] Actions,
        string? Icon = null,
        string? ControllerName = null,
        string? CreateActionName = null,
        bool IsLanding = false);

    private sealed record ActionSeed(string Code, string Name);

    private static readonly ModuleSeed[] Modules =
    [
        new("SYSTEM", "System", "Core system module",
            Actions:
            [
                new("VIEW", "View"),
                new("MANAGE", "Manage")
            ],
            Icon: "bi-speedometer2",
            ControllerName: null,
            CreateActionName: null,
            IsLanding: true),

        new("PRODUCTS", "Products", "Product catalog",
            Actions:
            [
                new("LIST", "List products"),
                new("CREATE", "Create product")
            ],
            Icon: "bi-box-seam",
            ControllerName: "Products"),

        new("CATEGORIES", "Categories", "Product categories",
            Actions:
            [
                new("LIST", "List categories"),
                new("CREATE", "Create category")
            ],
            Icon: "bi-tags",
            ControllerName: "Categories"),

        new("SUPPLIERS", "Proveedores", "Gestión de proveedores",
            Actions:
            [
                new("LIST", "Listar proveedores"),
                new("CREATE", "Crear proveedor")
            ],
            Icon: "bi-truck",
            ControllerName: "Suppliers"),

        new("USERS", "Users", "User management",
            Actions:
            [
                new("REGISTER", "Register user")
            ],
            Icon: "bi-people",
            ControllerName: "Account",
            CreateActionName: "Register"),

        new("MODULES", "Módulos", "Gestión de módulos del sistema",
            Actions:
            [
                new("LIST", "Listar módulos"),
                new("CREATE", "Crear módulo")
            ],
            Icon: "bi-grid-3x3-gap",
            ControllerName: "Modules"),

        new("ACTIONS", "Acciones", "Gestión de acciones por módulo",
            Actions:
            [
                new("LIST", "Listar acciones"),
                new("CREATE", "Crear acción")
            ],
            Icon: "bi-lightning",
            ControllerName: "AppActions"),

        new("ROLES", "Roles", "Gestión de roles",
            Actions:
            [
                new("LIST", "Listar roles"),
                new("CREATE", "Crear rol")
            ],
            Icon: "bi-shield-lock",
            ControllerName: "Roles"),

        new("PROFILES", "Perfiles", "Gestión de perfiles y permisos",
            Actions:
            [
                new("LIST", "Listar perfiles"),
                new("CREATE", "Crear perfil")
            ],
            Icon: "bi-person-badge",
            ControllerName: "Profiles")
    ];

    public static async Task SeedAsync(AppDbContext context)
    {
        var now = DateTime.UtcNow;
        const string createdBy = "module-seeder";

        foreach (var moduleSeed in Modules)
        {
            var module = await context.Modules
                .FirstOrDefaultAsync(m => m.Code == moduleSeed.Code);

            if (module is null)
            {
                module = new Module
                {
                    Code = moduleSeed.Code,
                    Name = moduleSeed.Name,
                    Description = moduleSeed.Description,
                    Icon = moduleSeed.Icon,
                    ControllerName = moduleSeed.ControllerName,
                    CreateActionName = moduleSeed.CreateActionName,
                    IsLanding = moduleSeed.IsLanding,
                    IsActive = true,
                    CreatedAt = now,
                    CreatedBy = createdBy
                };
                context.Modules.Add(module);
                await context.SaveChangesAsync();
            }
            else
            {
                // Keep navigation fields in sync if module already exists
                var changed = false;
                if (module.Icon != moduleSeed.Icon) { module.Icon = moduleSeed.Icon; changed = true; }
                if (module.ControllerName != moduleSeed.ControllerName) { module.ControllerName = moduleSeed.ControllerName; changed = true; }
                if (module.CreateActionName != moduleSeed.CreateActionName) { module.CreateActionName = moduleSeed.CreateActionName; changed = true; }
                if (module.IsLanding != moduleSeed.IsLanding) { module.IsLanding = moduleSeed.IsLanding; changed = true; }
                if (changed)
                {
                    module.UpdatedAt = now;
                    module.UpdatedBy = createdBy;
                }
            }

            foreach (var actionSeed in moduleSeed.Actions)
            {
                var action = await context.ActionModules
                    .Where(am => am.ModuleId == module.Id && am.Action.Code == actionSeed.Code)
                    .Select(am => am.Action)
                    .FirstOrDefaultAsync();

                if (action is null)
                {
                    action = new ActionEntity
                    {
                        Code = actionSeed.Code,
                        Name = actionSeed.Name,
                        IsActive = true,
                        CreatedAt = now,
                        CreatedBy = createdBy
                    };
                    context.Actions.Add(action);
                    await context.SaveChangesAsync();
                }

                var linkExists = await context.ActionModules.AnyAsync(am =>
                    am.ActionId == action.Id &&
                    am.ModuleId == module.Id &&
                    am.Estado == RelationStatuses.Active);

                if (linkExists)
                    continue;

                context.ActionModules.Add(new ActionModule
                {
                    ActionId = action.Id,
                    ModuleId = module.Id,
                    Estado = RelationStatuses.Active,
                    CreatedByUserId = SystemUserId,
                    CreatedAt = now
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
