using MediatR;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.DeleteSupplier;

public record DeleteSupplierCommand(int Id) : IRequest;
