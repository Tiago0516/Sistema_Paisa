using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Commands.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IProfileRepository _profileRepository;

    public UpdateProfileHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.Id);
        if (profile is null) return false;

        var moduleIds = request.ModuleIds.Distinct().ToList();

        profile.Name = request.Name.Trim();
        profile.Description = request.Description.Trim();
        profile.ModuleId = moduleIds.FirstOrDefault();
        profile.IsActive = request.IsActive;

        await _profileRepository.UpdateAsync(profile, moduleIds, request.ActionIds);
        return true;
    }
}
