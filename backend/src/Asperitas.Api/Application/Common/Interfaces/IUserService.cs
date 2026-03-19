using Asperitas.Api.Domain.Entities;

namespace Asperitas.Api.Application.Common.Interfaces;

public interface IUserService
{
    public Task CreateAsync(User user, CancellationToken cancellationToken);
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}
