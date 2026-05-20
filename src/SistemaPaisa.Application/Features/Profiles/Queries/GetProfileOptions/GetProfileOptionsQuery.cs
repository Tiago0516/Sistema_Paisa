using MediatR;

namespace SistemaPaisa.Application.Features.Profiles.Queries.GetProfileOptions;

public record GetProfileOptionsQuery : IRequest<IEnumerable<ProfileOptionDto>>;

public class ProfileOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
