namespace Librium.Domain.Books.Repositories;

public interface IBookRepository
{
    Task<Book?> GetBookById(Guid bookId);
    Task<IReadOnlyList<Book>> GetAllBooks(string? search);
    Task<bool> ExistBookAsync(string author, string title);
    Task<bool> SaveChangesAsync();
    void Add(Book book);
    void Delete(Book book);
}