namespace Librium.Application.Categories.DTOs;

public record CategoryResponseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
