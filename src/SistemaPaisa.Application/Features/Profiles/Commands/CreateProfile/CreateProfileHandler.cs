using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Commands.CreateProfile;

public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, int>
{
    private readonly IProfileRepository _profileRepository;

    public CreateProfileHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<int> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var moduleIds = request.ModuleIds.Distinct().ToList();
        var profile = new Profile
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            ModuleId = moduleIds.FirstOrDefault()
        };

        var created = await _profileRepository.AddAsync(profile, moduleIds, request.ActionIds);
        return created.Id;
    }
}
