namespace SistemaPaisa.Application.Common.Navigation;

/// <summary>
/// Rutas amigables (slug) por código de módulo. Fuente única para URLs y permisos de navegación.
/// </summary>
public static class ModuleRoutes
{
    public const string LandingSlug = "home";

    private static readonly Dictionary<string, string> CodeToSlug =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["SYSTEM"] = LandingSlug,
            ["PRODUCTS"] = "products",
            ["CATEGORIES"] = "categories",
            ["SUPPLIERS"] = "suppliers",
            ["USERS"] = "users",
            ["MODULES"] = "modules",
            ["ACTIONS"] = "actions",
            ["ROLES"] = "roles",
            ["PROFILES"] = "profiles"
        };

    private static readonly Dictionary<string, string> SlugToCode =
        CodeToSlug.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.OrdinalIgnoreCase);

    public static bool TryGetSlug(string moduleCode, out string slug) =>
        CodeToSlug.TryGetValue(moduleCode, out slug!);

    public static bool TryGetCode(string slug, out string moduleCode) =>
        SlugToCode.TryGetValue(slug, out moduleCode!);

    public static string GetWorkspacePath(string moduleCode)
    {
        if (!TryGetSlug(moduleCode, out var slug))
            return "/home";

        return slug == LandingSlug ? "/home" : $"/{slug}";
    }

    public static string GetWorkspacePathFromSlug(string slug) =>
        string.Equals(slug, LandingSlug, StringComparison.OrdinalIgnoreCase) ? "/home" : $"/{slug}";

    public static IReadOnlyCollection<string> AllSlugs => SlugToCode.Keys.ToList();
}
