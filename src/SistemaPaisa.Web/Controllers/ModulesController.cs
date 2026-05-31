using MediatR;
using SistemaPaisa.Application.Common.Navigation;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Features.Modules.Commands.CreateModule;
using SistemaPaisa.Application.Features.Modules.Commands.DeleteModule;
using SistemaPaisa.Application.Features.Modules.Commands.UpdateModule;
using SistemaPaisa.Application.Features.Modules.Queries.GetModuleById;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

[Route("modules")]
[RequireModuleAccess("MODULES")]
public class ModulesController : Controller
{
    private readonly IMediator _mediator;

    public ModulesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("create")]
    [RequireModuleAccess("MODULES", PermissionCodes.Create)]
    public IActionResult Create() =>
        Request.IsModalRequest()
            ? PartialView("_CreateForm", new CreateModuleCommand())
            : View(new CreateModuleCommand());

    [HttpPost("create"), ValidateAntiForgeryToken]
    [RequireModuleAccess("MODULES", PermissionCodes.Create)]
    public async Task<IActionResult> Create(CreateModuleCommand command)
    {
        if (!ModelState.IsValid)
        {
            if (Request.IsModalRequest())
                return PartialView("_CreateForm", command);
            return View(command);
        }

        await _mediator.Send(command);
        if (Request.IsModalRequest())
            return Json(new { success = true });

        return Redirect(ModuleRoutes.GetWorkspacePath("MODULES"));
    }

    [HttpGet("edit/{id:int}")]
    [RequireModuleAccess("MODULES", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(int id)
    {
        var module = await _mediator.Send(new GetModuleByIdQuery(id));
        if (module is null) return NotFound();

        var command = new UpdateModuleCommand
        {
            Id = module.Id,
            Name = module.Name,
            Description = module.Description,
            Code = module.Code,
            IsActive = module.IsActive
        };
        return View(command);
    }

    [HttpPost("edit/{id:int}"), ValidateAntiForgeryToken]
    [RequireModuleAccess("MODULES", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(UpdateModuleCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return Redirect(ModuleRoutes.GetWorkspacePath("MODULES"));
    }

    [HttpGet("delete/{id:int}")]
    [RequireModuleAccess("MODULES", PermissionCodes.Manage)]
    public async Task<IActionResult> Delete(int id)
    {
        var module = await _mediator.Send(new GetModuleByIdQuery(id));
        if (module is null) return NotFound();
        return View(module);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete"), ValidateAntiForgeryToken]
    [RequireModuleAccess("MODULES", PermissionCodes.Manage)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteModuleCommand(id));
        return Redirect(ModuleRoutes.GetWorkspacePath("MODULES"));
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var module = await _mediator.Send(new GetModuleByIdQuery(id));
        if (module is null) return NotFound();
        return View(module);
    }
}
