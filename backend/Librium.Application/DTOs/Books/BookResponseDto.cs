namespace Librium.Application.DTOs.Books;

public class BookResponseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public List<string> Categories { get; set; } = new();
    public string? Content { get; set; }
    public int PublishedYear { get; set; }
}