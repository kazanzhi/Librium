namespace Librium.Application.DTOs.Comments;

public class CommentResponseDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEdited { get; set; }
}