using MediatR;

namespace SistemaPaisa.Application.Features.Profiles.Commands.DeleteProfile;

public record DeleteProfileCommand(int Id) : IRequest<bool>;
