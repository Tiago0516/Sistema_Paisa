using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, bool>
{
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierHandler(ISupplierRepository supplierRepository) =>
        _supplierRepository = supplierRepository;

    public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);
        if (supplier is null) return false;

        supplier.Name = request.Name;
        supplier.Email = request.Email;
        supplier.IsActive = request.IsActive;

        await _supplierRepository.UpdateAsync(supplier);
        return true;
    }
}
