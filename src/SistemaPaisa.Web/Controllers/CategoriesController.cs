using MediatR;
using SistemaPaisa.Application.Common.Navigation;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Features.Categories.Commands.CreateCategory;
using SistemaPaisa.Application.Features.Categories.Commands.DeleteCategory;
using SistemaPaisa.Application.Features.Categories.Commands.UpdateCategory;
using SistemaPaisa.Application.Features.Categories.Queries.GetCategoryById;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

[Route("categories")]
[RequireModuleAccess("CATEGORIES")]
public class CategoriesController : Controller
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("create")]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Create)]
    public IActionResult Create()
    {
        var command = new CreateCategoryCommand();
        if (Request.IsModalRequest())
            return PartialView("_CreateForm", command);

        return View(command);
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Create)]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
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

        return Redirect(ModuleRoutes.GetWorkspacePath("CATEGORIES"));
    }

    [HttpGet("edit/{id:int}")]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();

        var command = new UpdateCategoryCommand(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive);

        return View(command);
    }

    [HttpPost("edit/{id:int}"), ValidateAntiForgeryToken]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Manage)]
    public async Task<IActionResult> Edit(UpdateCategoryCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return Redirect(ModuleRoutes.GetWorkspacePath("CATEGORIES"));
    }

    [HttpGet("delete/{id:int}")]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Manage)]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete"), ValidateAntiForgeryToken]
    [RequireModuleAccess("CATEGORIES", PermissionCodes.Manage)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return Redirect(ModuleRoutes.GetWorkspacePath("CATEGORIES"));
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();
        return View(category);
    }
}
