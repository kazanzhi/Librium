namespace Librium.Domain.Libraries;
public class LibraryBook
{
    private LibraryBook() { }

    public Guid BookId { get; private set; }
    public Guid UserLibraryId { get; private set; }

    internal LibraryBook(Guid bookId)
    {
        if (bookId == Guid.Empty)
            throw new ArgumentException("BookId is required.");

        BookId = bookId;
    }
}
