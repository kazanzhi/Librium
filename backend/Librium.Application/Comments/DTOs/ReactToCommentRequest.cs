using Librium.Domain.Comments.Enums;

namespace Librium.Application.Comments.DTOs;

public class ReactToCommentRequest
{
    public ReactionType ReactionType { get; set; }
}
