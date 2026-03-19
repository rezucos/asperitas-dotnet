using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Domain.Entities;
using Asperitas.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Asperitas.Api.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<User> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(
                u => u.Username == username && u.Password == password,
                cancellationToken: cancellationToken
            )
            ?? throw new UnauthorizedAccessException("Invalid username or password");
    }
}
