using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Web.Extensions;
using SistemaPaisa.Application.Features.Categories.Commands.CreateCategory;
using SistemaPaisa.Application.Features.Categories.Commands.DeleteCategory;
using SistemaPaisa.Application.Features.Categories.Commands.UpdateCategory;
using SistemaPaisa.Application.Features.Categories.Queries.GetAllCategories;
using SistemaPaisa.Application.Features.Categories.Queries.GetCategoryById;

namespace SistemaPaisa.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "CATEGORIES" });

    public IActionResult Create()
    {
        var command = new CreateCategoryCommand();
        if (Request.IsModalRequest())
            return PartialView("_CreateForm", command);

        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
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

        return RedirectToAction("Index", "Home", new { module = "CATEGORIES" });
    }

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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCategoryCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction("Index", "Home", new { module = "CATEGORIES" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return RedirectToAction("Index", "Home", new { module = "CATEGORIES" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (category is null) return NotFound();
        return View(category);
    }
}
