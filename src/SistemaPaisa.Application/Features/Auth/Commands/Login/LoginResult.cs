namespace SistemaPaisa.Application.Features.Auth.Commands.Login;

public class LoginResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public int UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public int ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;

    public static LoginResult Failed(string message) =>
        new() { Success = false, ErrorMessage = message };

    public static LoginResult Ok(
        int userId,
        string fullName,
        string email,
        int roleId,
        string roleName,
        int clientId,
        string clientName) =>
        new()
        {
            Success = true,
            UserId = userId,
            FullName = fullName,
            Email = email,
            RoleId = roleId,
            RoleName = roleName,
            ClientId = clientId,
            ClientName = clientName
        };
}
