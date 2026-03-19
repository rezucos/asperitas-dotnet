using Asperitas.Api.Application.Commands.Posts;
using Asperitas.Api.Application.Commands.Users;
using Asperitas.Api.Presentation.Contracts.Requests;

namespace Asperitas.Api.Presentation.Mapping;

public static class ApiContractToCommandMapper
{
    public static CreateUserCommand ToCreateUserCommand(this CreateUserRequest request)
    {
        return new CreateUserCommand(
            request.Username,
            request.Password
        );
    }

    public static CreatePostCommand ToCreatePostCommand(this CreatePostRequest request)
    {
        return new CreatePostCommand(
            request.Title,
            request.Text,
            request.Url,
            request.Type,
            request.Category
        );
    }

    public static LoginCommand ToLoginCommand(this LoginRequest request)
    {
        return new LoginCommand(
            request.Username,
            request.Password
        );
    }
}
