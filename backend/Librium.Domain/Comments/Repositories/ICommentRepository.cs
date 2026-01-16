namespace Librium.Domain.Comments.Repositories;
public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetByBookIdAsync(Guid bookId);
    Task<Comment?> GetByIdAsync(Guid commentId);
    void Remove(Comment comment);
    void Add(Comment comment);
    Task<bool> SaveChangesAsync();
}
