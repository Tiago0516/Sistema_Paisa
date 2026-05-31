using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQuery(int Id) : IRequest<SupplierDto?>;
