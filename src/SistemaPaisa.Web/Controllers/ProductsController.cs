using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Web.Extensions;
using SistemaPaisa.Application.Features.Categories.Queries.GetAllCategories;
using SistemaPaisa.Application.Features.Products.Commands.CreateProduct;
using SistemaPaisa.Application.Features.Products.Commands.DeleteProduct;
using SistemaPaisa.Application.Features.Products.Commands.UpdateProduct;
using SistemaPaisa.Application.Features.Products.Queries.GetAllProducts;
using SistemaPaisa.Application.Features.Products.Queries.GetProductById;

namespace SistemaPaisa.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator) => _mediator = mediator;

    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { module = "PRODUCTS" });

    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        var command = new CreateProductCommand(string.Empty, string.Empty, 0, 0, null);
        if (Request.IsModalRequest())
            return PartialView("_CreateForm", command);

        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(command.CategoryId);
            if (Request.IsModalRequest())
                return PartialView("_CreateForm", command);

            return View(command);
        }

        await _mediator.Send(command);
        if (Request.IsModalRequest())
            return Json(new { success = true });

        return RedirectToAction("Index", "Home", new { module = "PRODUCTS" });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product is null) return NotFound();

        var command = new UpdateProductCommand(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.IsActive,
            product.CategoryId);

        await LoadCategoriesAsync(product.CategoryId);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductCommand command)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(command.CategoryId);
            return View(command);
        }

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();

        return RedirectToAction("Index", "Home", new { module = "PRODUCTS" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product is null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return RedirectToAction("Index", "Home", new { module = "PRODUCTS" });
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product is null) return NotFound();
        return View(product);
    }

    private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedCategoryId.HasValue && selectedCategoryId.Value == c.Id
        }).ToList();
    }
}
