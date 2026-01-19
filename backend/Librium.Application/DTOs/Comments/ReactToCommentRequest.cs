using Librium.Domain.Comments.Enums;

namespace Librium.Application.DTOs.Comments;

public class ReactToCommentRequest
{
    public ReactionType ReactionType { get; set; }
}
