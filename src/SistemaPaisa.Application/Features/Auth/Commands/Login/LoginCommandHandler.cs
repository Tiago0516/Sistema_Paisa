using MediatR;
using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());

        if (user is null)
            return LoginResult.Failed("Invalid email or password.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return LoginResult.Failed("Invalid email or password.");

        if (!user.Role.IsActive)
            return LoginResult.Failed("The assigned role is not active.");

        if (!user.Role.Client.IsActive)
            return LoginResult.Failed("The client associated with the role is not active.");

        var fullName = $"{user.FirstName} {user.LastName}".Trim();
        return LoginResult.Ok(
            user.Id,
            fullName,
            user.Email,
            user.Role.Id,
            user.Role.Name,
            user.Role.Client.Id,
            user.Role.Client.Name);
    }
}
