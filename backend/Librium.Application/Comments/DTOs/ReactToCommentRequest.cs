using Librium.Domain.Comments.Enums;

namespace Librium.Application.Comments.DTOs;

public record ReactToCommentRequest
{
    public ReactionType ReactionType { get; set; }
}
