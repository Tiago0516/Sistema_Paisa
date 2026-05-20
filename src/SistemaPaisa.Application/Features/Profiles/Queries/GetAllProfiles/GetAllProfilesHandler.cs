using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetAllProfiles;

public class GetAllProfilesHandler : IRequestHandler<GetAllProfilesQuery, IEnumerable<ProfileDto>>
{
    private readonly IProfileRepository _profileRepository;

    public GetAllProfilesHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<IEnumerable<ProfileDto>> Handle(
        GetAllProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var profiles = await _profileRepository.GetAllAsync();
        return profiles.Select(Map).ToList();
    }

    internal static ProfileDto Map(Profile profile)
    {
        var moduleNames = profile.ProfileModules.Count > 0
            ? profile.ProfileModules.Select(pm => pm.Module.Name).ToList()
            : profile.Module is not null ? [profile.Module.Name] : [];

        var actionNames = profile.ProfileActions.Count > 0
            ? profile.ProfileActions.Select(pa => pa.Action.Name).ToList()
            : [];

        var moduleIds = profile.ProfileModules.Count > 0
            ? profile.ProfileModules.Select(pm => pm.ModuleId).ToList()
            : profile.ModuleId > 0 ? [profile.ModuleId] : [];

        return new ProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Description = profile.Description,
            ModuleId = moduleIds.FirstOrDefault(),
            ModuleName = moduleNames.FirstOrDefault() ?? "—",
            ModuleIds = moduleIds,
            ModulesSummary = moduleNames.Count == 0 ? "—" : string.Join(", ", moduleNames),
            IsActive = profile.IsActive,
            ActionIds = profile.ProfileActions.Select(pa => pa.ActionId).ToList(),
            ActionsSummary = actionNames.Count == 0
                ? "Todas las acciones por módulo"
                : string.Join(", ", actionNames)
        };
    }
}
