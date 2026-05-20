using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Navigation.Queries.GetNavigationMenu;

public record GetNavigationMenuQuery : IRequest<NavigationMenuDto>;
