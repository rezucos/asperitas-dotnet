using Asperitas.Api.Domain.Entities;

namespace Asperitas.Api.Application.Common.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}
