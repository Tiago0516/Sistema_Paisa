using MediatR;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Navigation.Queries.GetNavigationMenu;

public class GetNavigationMenuHandler : IRequestHandler<GetNavigationMenuQuery, NavigationMenuDto>
{
    private readonly IPermissionService _permissionService;

    public GetNavigationMenuHandler(IPermissionService permissionService) =>
        _permissionService = permissionService;

    public async Task<NavigationMenuDto> Handle(
        GetNavigationMenuQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionService.GetCurrentPermissionsAsync(cancellationToken);

        if (permissions is null)
            return new NavigationMenuDto { Modules = [] };

        var items = permissions.Modules.Select(m => new NavigationModuleDto
        {
            Id = m.Id,
            Name = m.Name,
            Code = m.Code,
            Icon = m.Icon ?? NavigationUrlResolver.DefaultIcon,
            Url = NavigationUrlResolver.ResolveModuleUrl(m.Code),
            ControllerName = m.ControllerName,
            CreateActionName = m.CreateActionName,
            IsLanding = m.IsLanding,
            Actions = m.Actions.Select(a => new NavigationActionDto
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code,
                Url = NavigationUrlResolver.ResolveActionUrl(m.Code, a.Code)
            }).ToList()
        }).ToList();

        return new NavigationMenuDto { Modules = items };
    }
}
