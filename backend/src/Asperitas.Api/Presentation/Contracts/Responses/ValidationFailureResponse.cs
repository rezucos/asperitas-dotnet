namespace Asperitas.Api.Presentation.Contracts.Responses;

public class ValidationFailureResponse
{
    public List<string> Errors { get; init; } = [];
}
