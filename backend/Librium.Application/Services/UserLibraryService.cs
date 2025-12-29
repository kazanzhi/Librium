using Librium.Domain.Books.Repositories;
using Librium.Domain.Common;
using Librium.Domain.DTOs.Books;
using Librium.Domain.Users.Models;
using Librium.Domain.Users.Repositories;
using Librium.Domain.Users.Services;

namespace Librium.Application.Services;

public class UserLibraryService : IUserLibraryService
{
    private readonly IUserLibraryRepository _libraryRepository;
    private readonly IBookRepository _bookRepository;

    public UserLibraryService(IUserLibraryRepository libraryRepository, IBookRepository bookRepository)
    {
        _libraryRepository = libraryRepository;
        _bookRepository = bookRepository;
    }

    public async Task<ValueOrResult> AddBookAsync(Guid userId, Guid bookId)
    {
        var book = await _bookRepository.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        var library = await _libraryRepository.GetByUserIdAsync(userId);

        if (library is null)
        {
            var createResult = UserLibrary.Create(userId);
            if (!createResult.IsSuccess)
                return createResult;

            library = createResult.Value!;
            await _libraryRepository.Add(library);
        }

        var result = library.AddBook(bookId);
        if (!result.IsSuccess)
            return result;

        await _libraryRepository.SaveChanges();
        return ValueOrResult.Success();
    }

    public async Task<List<BookResponseDto>> GetUserLibraryAsync(Guid userId)
    {
        var library = await _libraryRepository.GetByUserIdAsync(userId);
        if (library is null)
            return [];

        var books = await _bookRepository.GetByIdsAsync(
            library.Books.Select(b => b.BookId));

        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Content = b.Content,
            PublishedYear = b.PublishedYear,
            Categories = b.Categories.Select(c => c.Name).ToList()
        }).ToList();
    }

    public async Task<ValueOrResult> RemoveBookAsync(Guid userId, Guid bookId)
    {
        var library = await _libraryRepository.GetByUserIdAsync(userId);
        if (library is null)
            return ValueOrResult.Failure("User library not found.");

        var result = library.RemoveBook(bookId);
        if (!result.IsSuccess)
            return result;

        await _libraryRepository.SaveChanges();
        return ValueOrResult.Success();
    }
}
