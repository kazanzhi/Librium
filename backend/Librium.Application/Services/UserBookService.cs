using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Application.Services;
public class UserBookService : IUserBookService
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IBookRepository _bookRepository;
    public UserBookService(IUserBookRepository userBookRepository, IBookRepository bookRepository)
    {
        _userBookRepository = userBookRepository;
        _bookRepository = bookRepository;
    }

    public async Task<ValueOrResult> AddUserBookAsync(string userId, int bookId)    
    {
        var book = await _bookRepository.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found");

        var existing = await _userBookRepository.GetUserBookById(userId, bookId);

        if (existing is not null)
            return ValueOrResult.Failure("Book already added to user library.");

        var userBookResult = UserBook.Create(userId, bookId);
        if (!userBookResult.isSuccess)
            return ValueOrResult.Failure(userBookResult.ErrorMessage!);

        await _userBookRepository.AddUserBook(userBookResult.Value!);
        await _userBookRepository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<UserBook>> GetUserBooksAsync(string userId)
    {
        return await _userBookRepository.GetAllUserBooks(userId);
    }

    public async Task<ValueOrResult> RemoveUserBookAsync(string userId, int bookId)
    {
        var userBookExists = await _userBookRepository.GetUserBookById(userId, bookId);
        if (userBookExists is null)
            return ValueOrResult.Failure("The user does not have such a book.");

        await _userBookRepository.Delete(userBookExists);
        await _userBookRepository.SaveChanges();

        return ValueOrResult.Success();
    }
}
