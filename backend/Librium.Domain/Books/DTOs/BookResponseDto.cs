using Librium.Domain.Books.Models;

namespace Librium.Domain.DTOs.Books;

public class BookResponseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public List<string> BookCategories { get; set; } = new();
    public string? Content { get; set; }
    public int PublishedYear { get; set; }
}
