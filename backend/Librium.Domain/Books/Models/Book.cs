using Librium.Domain.Common;
using Librium.Domain.Users.Models;

namespace Librium.Domain.Books.Models;

public class Book
{
    private readonly List<Category> _categories = new();
    private Book() { }
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int PublishedYear { get; private set; }
    public IReadOnlyCollection<Category> Categories => _categories;

    public static ValueOrResult<Book> Create(string title, string author, string content, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ValueOrResult<Book>.Failure("Title is required.");

        if (string.IsNullOrWhiteSpace(author))
            return ValueOrResult<Book>.Failure("Author is required.");

        if (string.IsNullOrWhiteSpace(content))
            return ValueOrResult<Book>.Failure("Content is required.");

        if (publishedYear < 0)
            return ValueOrResult<Book>.Failure("Invalid published year.");

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Author = author.Trim(),
            Content = content.Trim(),
            PublishedYear = publishedYear
        };

        return ValueOrResult<Book>.Success(book);
    }

    public ValueOrResult Update(string title, string author, string content, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ValueOrResult.Failure("Title is required.");

        if (string.IsNullOrWhiteSpace(author))
            return ValueOrResult<Book>.Failure("Author is required.");

        if (string.IsNullOrWhiteSpace(content))
            return ValueOrResult<Book>.Failure("Content is required.");

        if (publishedYear < 0)
            return ValueOrResult<Book>.Failure("Invalid published year.");

        Title = title.Trim();
        Author = author.Trim();
        PublishedYear = publishedYear;
        Content = content.Trim();

        return ValueOrResult.Success();
    }
    
    public ValueOrResult AddCategory(Category category)
    {
        if (category is null)
            return ValueOrResult.Failure("Category is required.");

        if (_categories.Any(c => c.Id == category.Id))
            return ValueOrResult.Failure("Category already assigned.");

        _categories.Add(category);

        return ValueOrResult.Success();
    }

    public ValueOrResult RemoveCategory(Category category)
    {
        if (category is null)
            return ValueOrResult.Failure("Category is required.");

        var existing = _categories.FirstOrDefault(c => c.Id == category.Id);

        if (existing is null)
            return ValueOrResult.Failure("Category not found.");

        _categories.Remove(category);

        return ValueOrResult.Success();
    }
}
