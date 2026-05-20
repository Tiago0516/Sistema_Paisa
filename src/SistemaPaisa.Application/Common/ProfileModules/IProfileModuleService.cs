using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ProfileModules;

public interface IProfileModuleService
{
    Task<IReadOnlyList<ProfileModuleInfo>> GetByProfileIdAsync(
        int profileId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfileModuleInfo>> GetForCurrentUserAsync(
        CancellationToken cancellationToken = default);
}
