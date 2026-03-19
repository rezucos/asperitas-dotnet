namespace Asperitas.Api.Presentation.Contracts.Responses;

public class ErrorResponse
{
    public required string Message { get; init; }
    public List<string> Errors { get; init; } = [];
}
