using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Common.Repositories;
using MediatR;

namespace Asperitas.Api.Application.Commands.Users;

public record LoginCommand(string Username, string Password) : IRequest<string>;

class LoginHandler(IUserRepository userRepository, ITokenService tokenService) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByCredentialsAsync(
            request.Username,
            request.Password,
            cancellationToken
        );

        return tokenService.GenerateToken(user);
    }
}
