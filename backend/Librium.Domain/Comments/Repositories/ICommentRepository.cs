namespace Librium.Domain.Comments.Repositories;
public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetByBookIdAsync(Guid bookId);
    Task<Comment?> GetByIdAsync(Guid commentId);
    Task<bool> SaveChangesAsync();
    void Delete(Comment comment);
    void Add(Comment comment);
}