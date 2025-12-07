using Librium.Domain.Common;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Books.Models;

public class Book
{
    private Book() { }
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public Guid CategoryId { get; private set; }
    public BookCategory BookCategory { get; private set; } = null!;
    public string Content { get; private set; } = string.Empty;
    public int PublishedYear { get; private set; }
    public ICollection<UserBook> UserBooks { get; private set; } = new List<UserBook>();

    public static ValueOrResult<Book> Create(string title, string author, Guid categoryId, string content, int publishedYear)
    {
        if (title is null)
            return ValueOrResult<Book>.Failure("Title is required.");

        if (author is null)
            return ValueOrResult<Book>.Failure("Author is required.");

        if (categoryId == Guid.Empty)
            return ValueOrResult<Book>.Failure("Category is required.");

        if (content is null)
            return ValueOrResult<Book>.Failure("Content is required.");

        if (publishedYear < 0)
            return ValueOrResult<Book>.Failure("Invalid published year.");

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Author = author.Trim(),
            Content = content.Trim(),
            PublishedYear = publishedYear,
            CategoryId = categoryId
        };

        return ValueOrResult<Book>.Success(book);
    }

    public ValueOrResult Update(string title, string author, string content, int publishedYear, BookCategory category)
    {
        if (string.IsNullOrEmpty(title))
            return ValueOrResult.Failure("Title is required.");

        if (string.IsNullOrEmpty(author))
            return ValueOrResult<Book>.Failure("Author is required.");

        if (string.IsNullOrEmpty(content))
            return ValueOrResult<Book>.Failure("Content is required.");

        if (publishedYear < 0)
            return ValueOrResult<Book>.Failure("Invalid published year.");

        Title = title.Trim();
        Author = author.Trim();
        PublishedYear = publishedYear;
        Content = content.Trim();
        BookCategory = category;

        return ValueOrResult.Success();
    }
}
