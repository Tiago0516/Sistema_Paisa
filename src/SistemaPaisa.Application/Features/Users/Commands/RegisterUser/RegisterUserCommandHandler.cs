using MediatR;
using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(email))
            return RegisterUserResult.Failed("El correo ya está registrado.");

        Role? role;
        if (request.RoleId > 0)
        {
            role = await _roleRepository.GetActiveByIdAsync(request.RoleId);
            if (role is null)
                return RegisterUserResult.Failed("El rol seleccionado no es válido.");
        }
        else
        {
            role = await _roleRepository.GetDefaultForRegistrationAsync();
            if (role is null)
                return RegisterUserResult.Failed("No hay un rol predeterminado configurado.");
        }

        if (request.ClientId > 0 && role.ClientId != request.ClientId)
            return RegisterUserResult.Failed("El rol seleccionado no pertenece a este cliente.");

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = role.Id,
            CreatedBy = string.IsNullOrWhiteSpace(request.CreatedBy) ? email : request.CreatedBy
        };

        var created = await _userRepository.AddAsync(user);
        return RegisterUserResult.Ok(created.Id);
    }
}
