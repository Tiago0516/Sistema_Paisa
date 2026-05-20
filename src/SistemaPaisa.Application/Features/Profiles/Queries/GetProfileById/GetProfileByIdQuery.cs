using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetProfileById;

public record GetProfileByIdQuery(int Id) : IRequest<ProfileDto?>;
