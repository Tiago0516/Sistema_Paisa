using MediatR;
using SistemaPaisa.Application.Common.Navigation;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Features.Suppliers.Commands.CreateSupplier;
using SistemaPaisa.Application.Features.Suppliers.Commands.DeleteSupplier;
using SistemaPaisa.Application.Features.Suppliers.Commands.UpdateSupplier;
using SistemaPaisa.Application.Features.Suppliers.Queries.GetSupplierById;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

[Route("suppliers")]
[RequireModuleAccess("SUPPLIERS")]
public class SuppliersController : Controller
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("create")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Create)]
    public IActionResult Create()
    {
        var command = new CreateSupplierCommand();
        if (Request.IsModalRequest())
            return PartialView("_CreateForm", command);

        return View(command);
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Create)]
    public async Task<IActionResult> Create(CreateSupplierCommand command)
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

        return Redirect(ModuleRoutes.GetWorkspacePath("SUPPLIERS"));
    }

    [HttpGet("edit/{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));
        if (supplier is null) return NotFound();

        var command = new UpdateSupplierCommand(
            supplier.Id,
            supplier.Name,
            supplier.Email,
            supplier.IsActive);

        return View(command);
    }

    [HttpPost("edit/{id:int}"), ValidateAntiForgeryToken]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(UpdateSupplierCommand command)
    {
        if (!ModelState.IsValid) return View(command);

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();

        return Redirect(ModuleRoutes.GetWorkspacePath("SUPPLIERS"));
    }

    [HttpGet("delete/{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));
        if (supplier is null) return NotFound();
        return View(supplier);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete"), ValidateAntiForgeryToken]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteSupplierCommand(id));
        return Redirect(ModuleRoutes.GetWorkspacePath("SUPPLIERS"));
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));
        if (supplier is null) return NotFound();
        return View(supplier);
    }
}
