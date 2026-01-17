using Librium.Domain.Common;

namespace Librium.Domain.Comments;

public class Comment
{
    private Comment() { }
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid BookId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; }


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
            IsEdited = false
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
}
