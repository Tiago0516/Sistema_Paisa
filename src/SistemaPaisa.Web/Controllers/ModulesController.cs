using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Features.Modules.Commands.CreateModule;
using SistemaPaisa.Application.Features.Modules.Commands.DeleteModule;
using SistemaPaisa.Application.Features.Modules.Commands.UpdateModule;
using SistemaPaisa.Application.Features.Modules.Queries.GetModuleById;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class ModulesController : Controller
{
    private readonly IMediator _mediator;

    public ModulesController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "MODULES" });

    public IActionResult Create() =>
        Request.IsModalRequest()
            ? PartialView("_CreateForm", new CreateModuleCommand())
            : View(new CreateModuleCommand());

    [HttpPost, ValidateAntiForgeryToken]
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

        return RedirectToAction("Index", "Home", new { module = "MODULES" });
    }

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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateModuleCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction("Index", "Home", new { module = "MODULES" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var module = await _mediator.Send(new GetModuleByIdQuery(id));
        if (module is null) return NotFound();
        return View(module);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteModuleCommand(id));
        return RedirectToAction("Index", "Home", new { module = "MODULES" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var module = await _mediator.Send(new GetModuleByIdQuery(id));
        if (module is null) return NotFound();
        return View(module);
    }
}
