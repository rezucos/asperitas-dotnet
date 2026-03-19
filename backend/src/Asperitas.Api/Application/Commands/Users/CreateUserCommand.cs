using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Domain.Entities;
using MediatR;

namespace Asperitas.Api.Application.Commands.Users;

public record CreateUserCommand(
    string Username,
    string Password
) : IRequest<string>;

public class CreateUserHandler(
    IUserService userService,
    ITokenService tokenService
) : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Password = request.Password,
            Username = request.Username,
            IsAdmin = false
        };

        await _userService.CreateAsync(user, cancellationToken);

        return _tokenService.GenerateToken(user);
    }
}
