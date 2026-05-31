using MediatR;
using SistemaPaisa.Application.Common.Users;

namespace SistemaPaisa.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRegistrationService _userRegistrationService;

    public RegisterUserCommandHandler(IUserRegistrationService userRegistrationService) =>
        _userRegistrationService = userRegistrationService;

    public Task<RegisterUserResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken) =>
        _userRegistrationService.RegisterAsync(request, cancellationToken);
}
