using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetAllSuppliersHandler(ISupplierRepository supplierRepository) =>
        _supplierRepository = supplierRepository;

    public async Task<IEnumerable<SupplierDto>> Handle(
        GetAllSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            IsActive = s.IsActive
        });
    }
}
