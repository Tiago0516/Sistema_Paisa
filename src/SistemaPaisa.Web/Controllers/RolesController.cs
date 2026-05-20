using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Features.Clients.Queries.GetAllClients;
using SistemaPaisa.Application.Features.Profiles.Queries.GetProfileOptions;
using SistemaPaisa.Application.Features.Roles.Commands.CreateRole;
using SistemaPaisa.Application.Features.Roles.Commands.DeleteRole;
using SistemaPaisa.Application.Features.Roles.Commands.UpdateRole;
using SistemaPaisa.Application.Features.Roles.Queries.GetRoleById;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class RolesController : Controller
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "ROLES" });

    public async Task<IActionResult> Create()
    {
        await LoadFormDataAsync();
        var command = new CreateRoleCommand();
        return Request.IsModalRequest() ? PartialView("_CreateForm", command) : View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadFormDataAsync(command.ProfileId, command.ClientId);
            if (Request.IsModalRequest())
                return PartialView("_CreateForm", command);
            return View(command);
        }

        await _mediator.Send(command);
        if (Request.IsModalRequest())
            return Json(new { success = true });

        return RedirectToAction("Index", "Home", new { module = "ROLES" });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        if (role is null) return NotFound();

        var command = new UpdateRoleCommand
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            ProfileId = role.ProfileId,
            ClientId = role.ClientId,
            IsActive = role.IsActive
        };
        await LoadFormDataAsync(role.ProfileId, role.ClientId);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateRoleCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadFormDataAsync(command.ProfileId, command.ClientId);
            return View(command);
        }

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction("Index", "Home", new { module = "ROLES" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        if (role is null) return NotFound();
        return View(role);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return RedirectToAction("Index", "Home", new { module = "ROLES" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        if (role is null) return NotFound();
        return View(role);
    }

    private async Task LoadFormDataAsync(int? selectedProfileId = null, int? selectedClientId = null)
    {
        var profiles = await _mediator.Send(new GetProfileOptionsQuery());
        var clients = await _mediator.Send(new GetAllClientsQuery());

        ViewBag.Profiles = profiles.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name,
            Selected = selectedProfileId.HasValue && selectedProfileId.Value == p.Id
        }).ToList();

        ViewBag.Clients = clients.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedClientId.HasValue && selectedClientId.Value == c.Id
        }).ToList();
    }
}
