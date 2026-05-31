using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Features.Suppliers.Commands.CreateSupplier;
using SistemaPaisa.Application.Features.Suppliers.Commands.UpdateSupplier;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

[Route("suppliers")]
[RequireModuleAccess("SUPPLIERS")]
public class SuppliersController : Controller
{
    [HttpGet("create")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Create)]
    public IActionResult Create()
    {
        var command = new CreateSupplierCommand();
        if (Request.IsModalRequest())
            return PartialView("_CreateForm", command);

        return View(command);
    }

    [HttpGet("edit/{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public IActionResult Edit(int id) =>
        View(new UpdateSupplierCommand { Id = id });

    [HttpGet("delete/{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    public IActionResult Delete(int id) =>
        View(id);

    [HttpGet("details/{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.View)]
    public IActionResult Details(int id) =>
        View(id);
}
