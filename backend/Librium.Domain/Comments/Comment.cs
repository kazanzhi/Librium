using Librium.Domain.Comments.Enums;
using Librium.Domain.Common;

namespace Librium.Domain.Comments;

public class Comment
{
    private readonly List<CommentReaction> _reactions = new();
    private Comment() { }
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid BookId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; }
    public int TotalLikes { get; private set; }
    public int TotalDislikes { get; private set; }

    public IReadOnlyCollection<CommentReaction> Reactions => _reactions;


    public static ValueOrResult<Comment> Create(Guid userId, Guid bookId, string content, DateTime createdAt)
    {
        if (userId == Guid.Empty)
        {
            return ValueOrResult<Comment>.Failure("UserId is required.");
        }

        if (bookId == Guid.Empty)
        {
            return ValueOrResult<Comment>.Failure("BookId is required.");
        }

        if(string.IsNullOrWhiteSpace(content) || content.Length < 3)
        {
            return ValueOrResult<Comment>.Failure("Comment must be at least 3 characters long.");
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BookId = bookId,
            Content = content.Trim(),
            CreatedAt = createdAt,
            IsEdited = false,
            TotalLikes = 0,
            TotalDislikes = 0
        };

        return ValueOrResult<Comment>.Success(comment);
    }

    public ValueOrResult Update(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length < 3)
        {
            return ValueOrResult<Comment>.Failure("Comment must be at least 3 characters long.");
        }

        Content = content.Trim();
        IsEdited = true;

        return ValueOrResult.Success();
    }

    public ValueOrResult React(Guid userId, ReactionType reactionType)
    {
        if (userId == Guid.Empty)
            return ValueOrResult.Failure("UserId is required.");

        var existing = _reactions.FirstOrDefault(u => u.UserId == userId);

        if (existing is null)
        {
            AddReaction(userId, reactionType);
            return ValueOrResult.Success();
        }

        if(existing.ReactionType == reactionType)
        {
            RemoveReaction(existing);
            return ValueOrResult.Success();
        }

        ChangeReaction(existing, reactionType);
        return ValueOrResult.Success();
    }

    private void ChangeReaction(CommentReaction reaction, ReactionType newType)
    {
        DecrementCounter(reaction.ReactionType);
        reaction.ChangeReaction(newType);
        IncrementCounter(newType);
    }

    private void RemoveReaction(CommentReaction reaction)
    {
        _reactions.Remove(reaction);
        DecrementCounter(reaction.ReactionType);
    }

    private void AddReaction(Guid userId, ReactionType reactionType)
    {
        _reactions.Add(new CommentReaction(userId, reactionType));
        IncrementCounter(reactionType);
    }

    private void IncrementCounter(ReactionType type)
    {
        if (type == ReactionType.Like)
            TotalLikes++;
        else
            TotalDislikes++;
    }

    private void DecrementCounter(ReactionType type)
    {
        if(type == ReactionType.Like)
            TotalLikes--;
        else
            TotalDislikes--;
    }
}