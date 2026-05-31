namespace SistemaPaisa.Application.Common.Navigation;

public static class NavigationUrlResolver
{
    public const string DefaultIcon = "bi-grid";

    public static string ResolveModuleUrl(string moduleCode) =>
        ModuleRoutes.GetWorkspacePath(moduleCode);

    public static string ResolveActionUrl(string moduleCode, string actionCode) =>
        ModuleRoutes.GetWorkspacePath(moduleCode);
}
