using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;
using Librium.Domain.Users.DTOs;
using Librium.Domain.Users.Repositories;

namespace Librium.Application.Services;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IBookRepository _bookRepository;

    public AppUserService(IAppUserRepository appUserRepository, IBookRepository bookRepository)
    {
        _appUserRepository = appUserRepository;
        _bookRepository = bookRepository;
    }

    public async Task<ValueOrResult> AddUserBookAsync(string userId, Guid bookId)
    {
        var book = await _bookRepository.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        var user = await _appUserRepository.GetAppUserById(userId);

        var result = user.AddBook(bookId);
        if (!result.IsSuccess)
            return ValueOrResult.Failure(result.ErrorMessage!);

        await _appUserRepository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<UserBookResponseDto>> GetUserBooksAsync(string userId)
    {
        var items = await _appUserRepository.GetAppUserBooks(userId);

        return items.Select(x => new UserBookResponseDto
        {
            BookId = x.BookId,
            Title = x.Book.Title,
            Author = x.Book.Author,
            AddedAt = x.AddedAt
        }).ToList();
    }

    public async Task<ValueOrResult> RemoveUserBookAsync(string userId, Guid bookId)
    {
        var user = await _appUserRepository.GetAppUserById(userId);
        var result = user.RemoveBook(bookId);
        if (!result.IsSuccess)
            return ValueOrResult.Failure(result.ErrorMessage!);

        await _appUserRepository.SaveChanges();

        return ValueOrResult.Success();
    }
}
