using Microsoft.AspNetCore.Identity;
using SistemaPaisa.Application.Common.Interfaces;

namespace SistemaPaisa.Infrastructure.Security;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password) =>
        _hasher.HashPassword(new object(), password);

    public bool Verify(string password, string hash) =>
        _hasher.VerifyHashedPassword(new object(), hash, password) != PasswordVerificationResult.Failed;
}
