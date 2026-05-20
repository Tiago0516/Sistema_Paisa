using MediatR;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Application.Features.AppActions.Queries.GetAllAppActions;
using SistemaPaisa.Application.Features.Categories.Queries.GetAllCategories;
using SistemaPaisa.Application.Features.Modules.Queries.GetAllModules;
using SistemaPaisa.Application.Features.Navigation.Queries.GetNavigationMenu;
using SistemaPaisa.Application.Features.Products.Queries.GetAllProducts;
using SistemaPaisa.Application.Features.Profiles.Queries.GetAllProfiles;
using SistemaPaisa.Application.Features.Roles.Queries.GetAllRoles;
using SistemaPaisa.Application.Features.Users.Queries.GetAllUsers;
using SistemaPaisa.Web.Models;

namespace SistemaPaisa.Web.Services;

public class ModuleWorkspaceBuilder
{
    private readonly IMediator _mediator;
    private readonly IPermissionService _permissionService;
    private readonly LinkGenerator _links;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Dictionary<string, Func<NavigationModuleDto, CancellationToken, Task<ModuleGridViewModel>>>? _builders;

    public ModuleWorkspaceBuilder(
        IMediator mediator,
        IPermissionService permissionService,
        LinkGenerator links,
        IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _permissionService = permissionService;
        _links = links;
        _httpContextAccessor = httpContextAccessor;
    }

    private string ActionUrl(string action, string controller, object? values = null) =>
        _links.GetPathByAction(
            _httpContextAccessor.HttpContext!,
            action,
            controller,
            values) ?? "/";

    private Dictionary<string, Func<NavigationModuleDto, CancellationToken, Task<ModuleGridViewModel>>> GetBuilders() =>
        _builders ??= new(StringComparer.OrdinalIgnoreCase)
        {
            ["Products"]   = (mod, ct) => BuildProductsGridAsync(mod, ct),
            ["Categories"] = (mod, ct) => BuildCategoriesGridAsync(mod, ct),
            ["Account"]    = (mod, ct) => BuildUsersGridAsync(mod, ct),
            ["Modules"]    = (mod, ct) => BuildModulesGridAsync(mod, ct),
            ["AppActions"] = (mod, ct) => BuildActionsGridAsync(mod, ct),
            ["Roles"]      = (mod, ct) => BuildRolesGridAsync(mod, ct),
            ["Profiles"]   = (mod, ct) => BuildProfilesGridAsync(mod, ct),
        };

    public async Task<WorkspacePageViewModel> BuildAsync(string? moduleCode, CancellationToken cancellationToken = default)
    {
        var menu = await _mediator.Send(new GetNavigationMenuQuery(), cancellationToken);
        var modules = menu.Modules;

        var selected = modules.FirstOrDefault(m =>
                string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase))
            ?? modules.FirstOrDefault(m => !m.IsLanding)
            ?? modules.FirstOrDefault();

        if (selected is null)
        {
            return new WorkspacePageViewModel
            {
                ActiveModuleCode = string.Empty,
                ActiveModuleName = "Inicio",
                Grid = new ModuleGridViewModel { Title = "Inicio" }
            };
        }

        ModuleGridViewModel grid;

        if (selected.IsLanding)
        {
            grid = BuildLandingGrid(modules);
        }
        else if (selected.ControllerName is not null
                 && GetBuilders().TryGetValue(selected.ControllerName, out var builder))
        {
            grid = await builder(selected, cancellationToken);
        }
        else
        {
            grid = BuildLandingGrid(modules);
        }

        grid = grid with
        {
            ModuleCode = selected.Code,
            AllowCreate = grid.AllowCreate && await CanCreateAsync(selected, cancellationToken),
            AllowView   = await CanViewAsync(selected.Code, cancellationToken),
            AllowEdit   = await CanEditAsync(selected.Code, cancellationToken),
            AllowDelete = await CanDeleteAsync(selected.Code, cancellationToken),
            CreateUrl   = await CanCreateAsync(selected, cancellationToken) ? grid.CreateUrl : string.Empty
        };

        return new WorkspacePageViewModel
        {
            ActiveModuleCode = selected.Code,
            ActiveModuleName = selected.Name,
            Grid = grid
        };
    }

    private async Task<ModuleGridViewModel> BuildProductsGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "Products"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Rows = products.Select(p => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["name"]        = p.Name,
                    ["description"] = Truncate(p.Description, 48),
                    ["price"]       = p.Price.ToString("C"),
                    ["stock"]       = p.Stock.ToString(),
                    ["category"]    = p.CategoryName ?? "—",
                    ["active"]      = p.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "Products", new { id = p.Id }),
                    EditUrl    = ActionUrl("Edit",    "Products", new { id = p.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "Products", new { id = p.Id })
                }
            }).ToList(),
            Columns =
            [
                new() { Key = "name",        Label = "Nombre" },
                new() { Key = "description", Label = "Descripción" },
                new() { Key = "price",       Label = "Precio" },
                new() { Key = "stock",       Label = "Stock" },
                new() { Key = "category",    Label = "Categoría" },
                new() { Key = "active",      Label = "Activo" }
            ]
        };
    }

    private async Task<ModuleGridViewModel> BuildCategoriesGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "Categories"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Rows = categories.Select(c => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["name"]        = c.Name,
                    ["description"] = Truncate(c.Description, 48),
                    ["active"]      = c.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "Categories", new { id = c.Id }),
                    EditUrl    = ActionUrl("Edit",    "Categories", new { id = c.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "Categories", new { id = c.Id })
                }
            }).ToList(),
            Columns =
            [
                new() { Key = "name",        Label = "Nombre" },
                new() { Key = "description", Label = "Descripción" },
                new() { Key = "active",      Label = "Activo" }
            ]
        };
    }

    private async Task<ModuleGridViewModel> BuildUsersGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Register", module.ControllerName ?? "Account"),
            AllowCreate = true,
            AllowView = false,
            AllowEdit = true,
            AllowDelete = false,
            Columns =
            [
                new() { Key = "name",   Label = "Nombre" },
                new() { Key = "email",  Label = "Correo" },
                new() { Key = "role",   Label = "Rol" },
                new() { Key = "client", Label = "Cliente" },
                new() { Key = "active", Label = "Activo" }
            ],
            Rows = users.Select(u => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["name"]   = $"{u.FirstName} {u.LastName}",
                    ["email"]  = u.Email,
                    ["role"]   = u.RoleName,
                    ["client"] = u.ClientName,
                    ["active"] = u.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    EditUrl = ActionUrl("EditRole", "Account", new { id = u.Id })
                }
            }).ToList()
        };
    }

    private async Task<ModuleGridViewModel> BuildModulesGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var modules = await _mediator.Send(new GetAllModulesQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "Modules"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Columns =
            [
                new() { Key = "code",        Label = "Código" },
                new() { Key = "name",        Label = "Nombre" },
                new() { Key = "description", Label = "Descripción" },
                new() { Key = "active",      Label = "Activo" }
            ],
            Rows = modules.Select(m => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["code"]        = m.Code,
                    ["name"]        = m.Name,
                    ["description"] = Truncate(m.Description, 48),
                    ["active"]      = m.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "Modules", new { id = m.Id }),
                    EditUrl    = ActionUrl("Edit",    "Modules", new { id = m.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "Modules", new { id = m.Id })
                }
            }).ToList()
        };
    }

    private async Task<ModuleGridViewModel> BuildActionsGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var actions = await _mediator.Send(new GetAllAppActionsQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "AppActions"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Columns =
            [
                new() { Key = "code",   Label = "Código" },
                new() { Key = "name",   Label = "Nombre" },
                new() { Key = "module", Label = "Módulo" },
                new() { Key = "active", Label = "Activo" }
            ],
            Rows = actions.Select(a => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["code"]   = a.Code,
                    ["name"]   = a.Name,
                    ["module"] = a.ModuleName,
                    ["active"] = a.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "AppActions", new { id = a.Id }),
                    EditUrl    = ActionUrl("Edit",    "AppActions", new { id = a.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "AppActions", new { id = a.Id })
                }
            }).ToList()
        };
    }

    private async Task<ModuleGridViewModel> BuildRolesGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var roles = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "Roles"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Columns =
            [
                new() { Key = "name",        Label = "Nombre" },
                new() { Key = "description", Label = "Descripción" },
                new() { Key = "profile",     Label = "Perfil" },
                new() { Key = "client",      Label = "Cliente" },
                new() { Key = "active",      Label = "Activo" }
            ],
            Rows = roles.Select(r => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["name"]        = r.Name,
                    ["description"] = Truncate(r.Description, 48),
                    ["profile"]     = r.ProfileName,
                    ["client"]      = r.ClientName,
                    ["active"]      = r.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "Roles", new { id = r.Id }),
                    EditUrl    = ActionUrl("Edit",    "Roles", new { id = r.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "Roles", new { id = r.Id })
                }
            }).ToList()
        };
    }

    private async Task<ModuleGridViewModel> BuildProfilesGridAsync(NavigationModuleDto module, CancellationToken cancellationToken)
    {
        var profiles = await _mediator.Send(new GetAllProfilesQuery(), cancellationToken);

        return new ModuleGridViewModel
        {
            Title = module.Name,
            ModuleCode = module.Code,
            CreateUrl = ActionUrl(module.CreateActionName ?? "Create", "Profiles"),
            AllowCreate = true,
            AllowView = true,
            AllowEdit = true,
            AllowDelete = true,
            Columns =
            [
                new() { Key = "name",        Label = "Nombre" },
                new() { Key = "module",      Label = "Módulo" },
                new() { Key = "description", Label = "Descripción" },
                new() { Key = "actions",     Label = "Acciones permitidas" },
                new() { Key = "active",      Label = "Activo" }
            ],
            Rows = profiles.Select(p => new ModuleGridRow
            {
                Cells = new Dictionary<string, string>
                {
                    ["name"]        = p.Name,
                    ["module"]      = p.ModulesSummary,
                    ["description"] = Truncate(p.Description, 48),
                    ["actions"]     = p.ActionsSummary,
                    ["active"]      = p.IsActive ? "Sí" : "No"
                },
                Actions = new ModuleGridRowActions
                {
                    DetailsUrl = ActionUrl("Details", "Profiles", new { id = p.Id }),
                    EditUrl    = ActionUrl("Edit",    "Profiles", new { id = p.Id }),
                    DeleteUrl  = ActionUrl("Delete",  "Profiles", new { id = p.Id })
                }
            }).ToList()
        };
    }

    private ModuleGridViewModel BuildLandingGrid(IReadOnlyList<NavigationModuleDto> modules) =>
        new()
        {
            Title = "Sistema",
            CreateLabel = string.Empty,
            CreateUrl = string.Empty,
            Columns =
            [
                new() { Key = "module",      Label = "Módulo" },
                new() { Key = "description", Label = "Descripción" }
            ],
            Rows = modules
                .Where(m => !m.IsLanding)
                .Select(m => new ModuleGridRow
                {
                    Cells = new Dictionary<string, string>
                    {
                        ["module"]      = m.Name,
                        ["description"] = $"Gestionar {m.Name}"
                    },
                    Actions = new ModuleGridRowActions
                    {
                        DetailsUrl = ActionUrl("Index", "Home", new { module = m.Code })
                    }
                })
                .ToList()
        };

    // Permission checks using action codes from DB — no module-name special casing
    private Task<bool> CanCreateAsync(NavigationModuleDto module, CancellationToken ct)
    {
        // The action name for creation may be "Register" or "Create"; both map to DB action codes.
        // We check CREATE, REGISTER (for user-registration flows), and MANAGE.
        return HasAnyActionAsync(
            module.Code,
            ct,
            PermissionCodes.Create,
            PermissionCodes.Register,
            PermissionCodes.Manage);
    }

    private Task<bool> CanViewAsync(string moduleCode, CancellationToken ct) =>
        HasAnyActionAsync(moduleCode, ct, PermissionCodes.View, PermissionCodes.List);

    private Task<bool> CanEditAsync(string moduleCode, CancellationToken ct) =>
        _permissionService.HasActionAsync(moduleCode, PermissionCodes.Manage, ct);

    private Task<bool> CanDeleteAsync(string moduleCode, CancellationToken ct) =>
        _permissionService.HasActionAsync(moduleCode, PermissionCodes.Manage, ct);

    private async Task<bool> HasAnyActionAsync(
        string moduleCode,
        CancellationToken ct,
        params string[] codes)
    {
        foreach (var code in codes)
        {
            if (await _permissionService.HasActionAsync(moduleCode, code, ct))
                return true;
        }
        return false;
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return "—";

        return value.Length <= maxLength ? value : $"{value[..(maxLength - 3)]}...";
    }
}
