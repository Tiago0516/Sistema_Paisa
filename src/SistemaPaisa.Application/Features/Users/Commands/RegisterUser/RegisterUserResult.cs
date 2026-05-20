namespace SistemaPaisa.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public int UserId { get; init; }

    public static RegisterUserResult Failed(string message) =>
        new() { Success = false, ErrorMessage = message };

    public static RegisterUserResult Ok(int userId) =>
        new() { Success = true, UserId = userId };
}
