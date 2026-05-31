using MediatR;
using SistemaPaisa.Application.Common.Auth;

namespace SistemaPaisa.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService) => _authService = authService;

    public Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        _authService.LoginAsync(request.Email, request.Password, cancellationToken);
}
