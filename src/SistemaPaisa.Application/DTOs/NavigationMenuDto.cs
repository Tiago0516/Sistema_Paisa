using SistemaPaisa.Application.Common.Navigation;

namespace SistemaPaisa.Application.DTOs;

public class NavigationMenuDto
{
    public IReadOnlyList<NavigationModuleDto> Modules { get; init; } = [];
}

public class NavigationModuleDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Icon { get; init; } = NavigationUrlResolver.DefaultIcon;
    public string? Url { get; init; }
    public string? ControllerName { get; init; }
    public string? CreateActionName { get; init; }
    public bool IsLanding { get; init; }
    public IReadOnlyList<NavigationActionDto> Actions { get; init; } = [];
}

public class NavigationActionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Url { get; init; }
}
