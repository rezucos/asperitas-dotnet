namespace Asperitas.Api.Presentation.Contracts.Requests;

public class CreatePostRequest
{
    public required string Category { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
