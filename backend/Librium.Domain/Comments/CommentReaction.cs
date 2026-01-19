using Librium.Domain.Comments.Enums;

namespace Librium.Domain.Comments;
public class CommentReaction
{
    private CommentReaction() { }
    public Guid Id { get; private set; }
    public Guid CommentId { get; private set; }
    public Guid UserId { get; private set; }
    public ReactionType ReactionType { get; private set; }

    internal CommentReaction(Guid userId, ReactionType reactionType)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ReactionType = reactionType;
    }

    internal void ChangeReaction(ReactionType newType)
    {
        ReactionType = newType;
    }
}
