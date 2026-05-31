using MediatR;
using SistemaPaisa.Application.Common.ModuleActions;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Application.Common.ProfileModules;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Navigation.Queries.GetNavigationMenu;

public class GetNavigationMenuHandler : IRequestHandler<GetNavigationMenuQuery, NavigationMenuDto>
{
    private readonly IProfileRoleService _profileRoleService;
    private readonly IProfileModuleService _profileModuleService;
    private readonly IModuleActionService _moduleActionService;

    public GetNavigationMenuHandler(
        IProfileRoleService profileRoleService,
        IProfileModuleService profileModuleService,
        IModuleActionService moduleActionService)
    {
        _profileRoleService = profileRoleService;
        _profileModuleService = profileModuleService;
        _moduleActionService = moduleActionService;
    }

    public async Task<NavigationMenuDto> Handle(
        GetNavigationMenuQuery request,
        CancellationToken cancellationToken)
    {
        _ = await _profileRoleService.GetForCurrentUserAsync(cancellationToken);

        var profileModules = await _profileModuleService.GetForCurrentUserAsync(cancellationToken);
        if (profileModules.Count == 0)
            return new NavigationMenuDto { Modules = [] };

        var items = new List<NavigationModuleDto>();

        foreach (var module in profileModules)
        {
            var actions = await _moduleActionService.GetForCurrentUserByModuleCodeAsync(
                module.Code,
                cancellationToken);

            items.Add(new NavigationModuleDto
            {
                Id = module.ModuleId,
                Name = module.Name,
                Code = module.Code,
                Icon = module.Icon ?? NavigationUrlResolver.DefaultIcon,
                Url = NavigationUrlResolver.ResolveModuleUrl(module.Code),
                ControllerName = module.ControllerName,
                CreateActionName = module.CreateActionName,
                IsLanding = module.IsLanding,
                Actions = actions.Select(a => new NavigationActionDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Code = a.Code,
                    Url = NavigationUrlResolver.ResolveActionUrl(module.Code, a.Code)
                }).ToList()
            });
        }

        return new NavigationMenuDto { Modules = items };
    }
}
