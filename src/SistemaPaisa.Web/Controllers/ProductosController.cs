using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Features.Categorias.Queries.GetAllCategorias;
using SistemaPaisa.Application.Features.Productos.Commands.CreateProducto;
using SistemaPaisa.Application.Features.Productos.Commands.DeleteProducto;
using SistemaPaisa.Application.Features.Productos.Commands.UpdateProducto;
using SistemaPaisa.Application.Features.Productos.Queries.GetAllProductos;
using SistemaPaisa.Application.Features.Productos.Queries.GetProductoById;

namespace SistemaPaisa.Web.Controllers;

public class ProductosController : Controller
{
    private readonly IMediator _mediator;
    public ProductosController(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> Index()
    {
        var productos = await _mediator.Send(new GetAllProductosQuery());
        return View(productos);
    }

    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductoCommand command)
    {
        if (!ModelState.IsValid)
        {
            await CargarCategoriasAsync(command.CategoriaId);
            return View(command);
        }
        await _mediator.Send(command);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _mediator.Send(new GetProductoByIdQuery(id));
        if (producto is null) return NotFound();
        var command = new UpdateProductoCommand(
            producto.Id, producto.Nombre, producto.Descripcion,
            producto.Precio, producto.Stock, producto.Activo, producto.CategoriaId);
        await CargarCategoriasAsync(producto.CategoriaId);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductoCommand command)
    {
        if (!ModelState.IsValid)
        {
            await CargarCategoriasAsync(command.CategoriaId);
            return View(command);
        }
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _mediator.Send(new GetProductoByIdQuery(id));
        if (producto is null) return NotFound();
        return View(producto);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteProductoCommand(id));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var producto = await _mediator.Send(new GetProductoByIdQuery(id));
        if (producto is null) return NotFound();
        return View(producto);
    }

    private async Task CargarCategoriasAsync(int? categoriaSeleccionadaId = null)
    {
        var categorias = await _mediator.Send(new GetAllCategoriasQuery());
        ViewBag.Categorias = categorias.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Nombre,
            Selected = categoriaSeleccionadaId.HasValue && categoriaSeleccionadaId.Value == c.Id
        }).ToList();
    }
}