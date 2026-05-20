using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetProfileOptions;

public class GetProfileOptionsHandler : IRequestHandler<GetProfileOptionsQuery, IEnumerable<ProfileOptionDto>>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfileOptionsHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<IEnumerable<ProfileOptionDto>> Handle(
        GetProfileOptionsQuery request,
        CancellationToken cancellationToken)
    {
        var profiles = await _profileRepository.GetAllActiveAsync();
        return profiles.Select(p => new ProfileOptionDto { Id = p.Id, Name = p.Name });
    }
}
