namespace Asperitas.Api.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Username { get; }
    bool IsAdmin { get; }
    bool IsAuthenticated { get; }
}
