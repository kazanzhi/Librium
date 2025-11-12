using Librium.Domain.Common;
using Librium.Domain.Entities.Books;
using Librium.Domain.Users.Models;
using System.Text.Json.Serialization;

namespace Librium.Domain.Books.Models;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int CategoryId { get; set; }
    public BookCategory BookCategory { get; set; }
    public string Content { get; set; }
    public int PublishedYear { get; set; }

    [JsonIgnore]
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();

    public static ValueOrResult<Book> Create(string title, string author, string category, string content, int publishedYear)
    {
        if (title is null)
            return ValueOrResult<Book>.Failure("Title is required.");

        if (author is null)
            return ValueOrResult<Book>.Failure("Author is required.");

        if (category is null)
            return ValueOrResult<Book>.Failure("Category is required.");

        if (content is null)
            return ValueOrResult<Book>.Failure("Content is required.");

        if (publishedYear < 0)
            return ValueOrResult<Book>.Failure("Invalid published year.");

        var book = new Book
        {
            Title = title.Trim(),
            Author = author.Trim(),
            Content = content.Trim(),
            PublishedYear = publishedYear
        };

        return ValueOrResult<Book>.Success(book);
    }
}
