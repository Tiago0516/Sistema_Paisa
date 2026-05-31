using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Application.Features.Suppliers.Commands.CreateSupplier;
using SistemaPaisa.Application.Features.Suppliers.Commands.DeleteSupplier;
using SistemaPaisa.Application.Features.Suppliers.Commands.UpdateSupplier;
using SistemaPaisa.Application.Features.Suppliers.Queries.GetAllSuppliers;
using SistemaPaisa.Application.Features.Suppliers.Queries.GetSupplierById;
using SistemaPaisa.Web.Api.Models;
using SistemaPaisa.Web.Authorization;

namespace SistemaPaisa.Web.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
[Produces("application/json")]
[RequireModuleAccess("SUPPLIERS")]
public class SuppliersApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersApiController(IMediator mediator) => _mediator = mediator;

    /// <summary>Lista todos los proveedores activos.</summary>
    [HttpGet]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.List)]
    [ProducesResponseType(typeof(IEnumerable<SupplierDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll(CancellationToken cancellationToken)
    {
        var suppliers = await _mediator.Send(new GetAllSuppliersQuery(), cancellationToken);
        return Ok(suppliers);
    }

    /// <summary>Obtiene un proveedor por id.</summary>
    [HttpGet("{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.View)]
    [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupplierDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    /// <summary>Crea un proveedor.</summary>
    [HttpPost]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Create)]
    [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SupplierDto>> Create(
        [FromBody] CreateSupplierRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var id = await _mediator.Send(new CreateSupplierCommand
        {
            Name = request.Name,
            Email = request.Email
        }, cancellationToken);

        var created = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    /// <summary>Actualiza un proveedor.</summary>
    [HttpPut("{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupplierDto>> Update(
        int id,
        [FromBody] UpdateSupplierRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var updated = await _mediator.Send(new UpdateSupplierCommand
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            IsActive = request.IsActive
        }, cancellationToken);

        if (!updated)
            return NotFound();

        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    /// <summary>Elimina un proveedor.</summary>
    [HttpDelete("{id:int}")]
    [RequireModuleAccess("SUPPLIERS", PermissionCodes.Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var exists = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
        if (exists is null)
            return NotFound();

        await _mediator.Send(new DeleteSupplierCommand(id), cancellationToken);
        return NoContent();
    }
}
