using Asperitas.Api.Domain.Entities;

namespace Asperitas.Api.Application.Common.Repositories;

public interface IUserRepository
{
    Task<User> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken);
}
