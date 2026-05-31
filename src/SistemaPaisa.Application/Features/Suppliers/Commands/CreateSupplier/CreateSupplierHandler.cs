using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, int>
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierHandler(ISupplierRepository supplierRepository) =>
        _supplierRepository = supplierRepository;

    public async Task<int> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier
        {
            Name = request.Name,
            Email = request.Email
        };

        await _supplierRepository.AddAsync(supplier);
        return supplier.Id;
    }
}
