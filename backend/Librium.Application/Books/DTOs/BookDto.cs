namespace Librium.Application.Books.DTOs;

public record BookDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Content { get; set; }
    public int PublishedYear { get; set; }
}
