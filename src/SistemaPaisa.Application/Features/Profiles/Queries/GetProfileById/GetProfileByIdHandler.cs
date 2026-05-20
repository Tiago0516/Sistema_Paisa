using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Application.Features.Profiles.Queries.GetAllProfiles;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetProfileById;

public class GetProfileByIdHandler : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfileByIdHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.Id);
        return profile is null ? null : GetAllProfilesHandler.Map(profile);
    }
}
