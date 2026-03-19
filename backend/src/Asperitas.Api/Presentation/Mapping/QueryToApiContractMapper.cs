using Asperitas.Api.Domain.Entities;
using Asperitas.Api.Presentation.Contracts.Responses;

namespace Asperitas.Api.Presentation.Mapping;

public static class QueryToApiContractMapper
{
    public static CreateUserResponse ToCreateUserResponse(this User user)
    {
        return new CreateUserResponse
        {
            Username = user.Username
        };
    }
}
