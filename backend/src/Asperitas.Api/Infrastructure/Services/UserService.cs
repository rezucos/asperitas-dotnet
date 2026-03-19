using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Domain.Entities;
using Asperitas.Api.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Asperitas.Api.Infrastructure.Services;

public class UserService(AppDbContext dbContext) : IUserService
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username,
            cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            var message = $"User with name {user.Username} already exists";
            throw new ValidationException(message,
            [
                new ValidationFailure(nameof(User), message)
            ]);
        }

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username,
            cancellationToken: cancellationToken);
    }
}
