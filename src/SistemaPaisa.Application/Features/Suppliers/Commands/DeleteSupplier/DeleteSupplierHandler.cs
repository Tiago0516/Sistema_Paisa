using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand>
{
    private readonly ISupplierRepository _supplierRepository;

    public DeleteSupplierHandler(ISupplierRepository supplierRepository) =>
        _supplierRepository = supplierRepository;

    public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken) =>
        await _supplierRepository.DeleteAsync(request.Id);
}
