namespace SistemaPaisa.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    int? RoleId { get; }
    int? ClientId { get; }
    bool IsAuthenticated { get; }
}
