using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetAllProfiles;

public record GetAllProfilesQuery : IRequest<IEnumerable<ProfileDto>>;
