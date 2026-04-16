using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Features.Categorias.Commands.CreateCategoria;
using SistemaPaisa.Application.Features.Categorias.Commands.DeleteCategoria;
using SistemaPaisa.Application.Features.Categorias.Commands.UpdateCategoria;
using SistemaPaisa.Application.Features.Categorias.Queries.GetAllCategorias;
using SistemaPaisa.Application.Features.Categorias.Queries.GetCategoriaById;

namespace SistemaPaisa.Web.Controllers;

public class CategoriasController : Controller
{
    private readonly IMediator _mediator;

    public CategoriasController(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> Index()
    {
        var categorias = await _mediator.Send(new GetAllCategoriasQuery());
        return View(categorias);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoriaCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        await _mediator.Send(command);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var categoria = await _mediator.Send(new GetCategoriaByIdQuery(id));
        if (categoria is null) return NotFound();

        var command = new UpdateCategoriaCommand(
            categoria.Id,
            categoria.Nombre,
            categoria.Descripcion,
            categoria.Activa);
        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCategoriaCommand command)
    {
        if (!ModelState.IsValid) return View(command);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _mediator.Send(new GetCategoriaByIdQuery(id));
        if (categoria is null) return NotFound();
        return View(categoria);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _mediator.Send(new DeleteCategoriaCommand(id));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var categoria = await _mediator.Send(new GetCategoriaByIdQuery(id));
        if (categoria is null) return NotFound();
        return View(categoria);
    }
}
