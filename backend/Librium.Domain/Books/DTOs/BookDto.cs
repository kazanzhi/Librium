namespace Librium.Domain.Books.DTOs;

public class BookDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Category { get; set; }
    public string? Content { get; set; }
    public int PublishedYear { get; set; }
}
