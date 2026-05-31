using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.Users;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserRegistrationService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResult> RegisterAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(email))
            return RegisterUserResult.Failed("El correo ya está registrado.");

        Role? role;
        if (command.RoleId > 0)
        {
            role = await _roleRepository.GetActiveByIdAsync(command.RoleId);
            if (role is null)
                return RegisterUserResult.Failed("El rol seleccionado no es válido.");
        }
        else
        {
            role = await _roleRepository.GetDefaultForRegistrationAsync();
            if (role is null)
                return RegisterUserResult.Failed("No hay un rol predeterminado configurado.");
        }

        if (command.ClientId > 0 && role.ClientId != command.ClientId)
            return RegisterUserResult.Failed("El rol seleccionado no pertenece a este cliente.");

        var user = new User
        {
            FirstName = command.FirstName.Trim(),
            LastName = command.LastName.Trim(),
            Email = email,
            PasswordHash = _passwordHasher.Hash(command.Password),
            RoleId = role.Id,
            CreatedBy = string.IsNullOrWhiteSpace(command.CreatedBy) ? email : command.CreatedBy
        };

        var created = await _userRepository.AddAsync(user);
        return RegisterUserResult.Ok(created.Id);
    }
}
