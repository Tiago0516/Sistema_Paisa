using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Features.Navigation.Queries.GetNavigationMenu;

namespace SistemaPaisa.Web.ViewComponents;

public class ModuleSidebarViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public ModuleSidebarViewComponent(IMediator mediator) => _mediator = mediator;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menu = await _mediator.Send(new GetNavigationMenuQuery());
        return View(menu);
    }
}
