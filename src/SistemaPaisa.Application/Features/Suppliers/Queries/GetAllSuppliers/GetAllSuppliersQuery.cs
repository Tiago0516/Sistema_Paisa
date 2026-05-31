using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>;
