namespace Librium.Domain.Users.DTOs;

public class UserBookResponseDto
{
    public Guid BookId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public DateTime AddedAt { get; set; }
}
