using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;
using Librium.Domain.Users.Models;
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
        var user = await _appUserRepository.GetUserWithBooks(userId);
        if (user is null)
            return ValueOrResult.Failure("User not found.");

        var book = await _bookRepository.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found");

        var result = user.AddBook(bookId);
        if (!result.IsSuccess)
            return ValueOrResult.Failure(result.ErrorMessage!);

        await _appUserRepository.SaveChangesAsync();

        return ValueOrResult.Success();
    }

    public async Task<List<UserBook>> GetUserBooksAsync(string userId)
    {
        var user = await _appUserRepository.GetUserWithBooks(userId);
        if (user is null)
            return new List<UserBook>();

        return user.UserBooks.ToList();
    }

    public async Task<ValueOrResult> RemoveUserBookAsync(string userId, Guid bookId)
    {
        var user = await _appUserRepository.GetUserWithBooks(userId);
        if (user is null)
            return ValueOrResult.Failure("User not found");

        var result = user.RemoveBook(bookId);
        if (!result.IsSuccess)
            return ValueOrResult.Failure(result.ErrorMessage!);
        
        await _appUserRepository.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
