using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Features.AppActions.Commands.CreateAppAction;
using SistemaPaisa.Application.Features.AppActions.Commands.DeleteAppAction;
using SistemaPaisa.Application.Features.AppActions.Commands.UpdateAppAction;
using SistemaPaisa.Application.Features.AppActions.Queries.GetAppActionById;
using SistemaPaisa.Application.Features.Modules.Queries.GetAllModulesForSelect;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class AppActionsController : Controller
{
    private readonly IMediator _mediator;

    public AppActionsController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "ACTIONS" });

    public async Task<IActionResult> Create()
    {
        await LoadModulesAsync();
        var command = new CreateAppActionCommand();
        return Request.IsModalRequest() ? PartialView("_CreateForm", command) : View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAppActionCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadModulesAsync(command.ModuleId);
            if (Request.IsModalRequest())
                return PartialView("_CreateForm", command);
            return View(command);
        }

        await _mediator.Send(command);
        if (Request.IsModalRequest())
            return Json(new { success = true });

        return RedirectToAction("Index", "Home", new { module = "ACTIONS" });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var action = await _mediator.Send(new GetAppActionByIdQuery(id));
        if (action is null) return NotFound();

        var command = new UpdateAppActionCommand
        {
            Id = action.Id,
            Name = action.Name,
            Code = action.Code,
            ModuleId = action.ModuleId,
            IsActive = action.IsActive
        };
        await LoadModulesAsync(action.ModuleId);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateAppActionCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadModulesAsync(command.ModuleId);
            return View(command);
        }

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction("Index", "Home", new { module = "ACTIONS" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var action = await _mediator.Send(new GetAppActionByIdQuery(id));
        if (action is null) return NotFound();
        return View(action);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteAppActionCommand(id));
        return RedirectToAction("Index", "Home", new { module = "ACTIONS" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var action = await _mediator.Send(new GetAppActionByIdQuery(id));
        if (action is null) return NotFound();
        return View(action);
    }

    private async Task LoadModulesAsync(int? selectedModuleId = null)
    {
        var modules = await _mediator.Send(new GetAllModulesForSelectQuery());
        ViewBag.Modules = modules.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = m.Name,
            Selected = selectedModuleId.HasValue && selectedModuleId.Value == m.Id
        }).ToList();
    }
}
