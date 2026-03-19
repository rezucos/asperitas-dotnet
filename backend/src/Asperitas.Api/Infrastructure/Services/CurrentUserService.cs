using System.Security.Claims;
using Asperitas.Api.Application.Common.Interfaces;

namespace Asperitas.Api.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => 
        httpContextAccessor.HttpContext?.User;

    public Guid? UserId => 
        Guid.TryParse(User?.FindFirstValue("id"), out var id) ? id : null;

    public string? Username => 
        User?.FindFirstValue("username");

    public bool IsAdmin => 
        User?.FindFirstValue("isAdmin") == "true";

    public bool IsAuthenticated => 
        User?.Identity?.IsAuthenticated ?? false;
}
