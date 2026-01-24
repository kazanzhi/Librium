using Librium.Application.Categories.DTOs;

namespace Librium.Application.Books.DTOs;

public record BookResponseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public IReadOnlyList<CategoryResponseDto> Categories { get; set; }
    public string? Content { get; set; }
    public int PublishedYear { get; set; }
}