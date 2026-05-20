using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Features.AppActions.Queries.GetActionsByModule;
using SistemaPaisa.Application.Features.Modules.Queries.GetAllModulesForSelect;
using SistemaPaisa.Application.Features.Profiles.Commands.CreateProfile;
using SistemaPaisa.Application.Features.Profiles.Commands.DeleteProfile;
using SistemaPaisa.Application.Features.Profiles.Commands.UpdateProfile;
using SistemaPaisa.Application.Features.Profiles.Queries.GetProfileById;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class ProfilesController : Controller
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "PROFILES" });

    public async Task<IActionResult> Create()
    {
        await LoadModulesAsync();
        var command = new CreateProfileCommand();
        return Request.IsModalRequest() ? PartialView("_CreateForm", command) : View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProfileCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadModulesAsync(command.ModuleIds, command.ActionIds);
            if (Request.IsModalRequest())
                return PartialView("_CreateForm", command);
            return View(command);
        }

        await _mediator.Send(command);
        if (Request.IsModalRequest())
            return Json(new { success = true });

        return RedirectToAction("Index", "Home", new { module = "PROFILES" });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id));
        if (profile is null) return NotFound();

        var command = new UpdateProfileCommand
        {
            Id = profile.Id,
            Name = profile.Name,
            Description = profile.Description,
            ModuleIds = profile.ModuleIds.ToList(),
            IsActive = profile.IsActive,
            ActionIds = profile.ActionIds.ToList()
        };

        await LoadModulesAsync(profile.ModuleIds, profile.ActionIds);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProfileCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadModulesAsync(command.ModuleIds, command.ActionIds);
            return View(command);
        }

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction("Index", "Home", new { module = "PROFILES" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id));
        if (profile is null) return NotFound();
        return View(profile);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteProfileCommand(id));
        return RedirectToAction("Index", "Home", new { module = "PROFILES" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id));
        if (profile is null) return NotFound();
        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> ActionsByModule(int moduleId)
    {
        var actions = await _mediator.Send(new GetActionsByModuleQuery(moduleId));
        return Json(actions);
    }

    private async Task LoadModulesAsync(
        IEnumerable<int>? selectedModuleIds = null,
        IEnumerable<int>? selectedActionIds = null)
    {
        var modules = await _mediator.Send(new GetAllModulesForSelectQuery());
        ViewBag.Modules = modules.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = m.Name
        }).ToList();

        ViewData["SelectedModuleIds"] = selectedModuleIds?.ToList() ?? [];
        ViewData["SelectedActionIds"] = selectedActionIds?.ToList() ?? [];
    }
}
