using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler : IRequestHandler<DeleteProfileCommand, bool>
{
    private readonly IProfileRepository _profileRepository;

    public DeleteProfileHandler(IProfileRepository profileRepository) =>
        _profileRepository = profileRepository;

    public async Task<bool> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.Id);
        if (profile is null) return false;

        await _profileRepository.DeleteAsync(request.Id);
        return true;
    }
}
