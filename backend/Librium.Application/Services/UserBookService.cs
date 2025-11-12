using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;
using Librium.Domain.Users.Models;

namespace Librium.Application.Services;
public class UserBookService : IUserBookService
{
    private readonly IUserBookRepository _repository;
    public UserBookService(IUserBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<ValueOrResult> AddUserBookAsync(string userId, int bookId)
    {
        var existingUserBook = await _repository.GetUserBookById(userId, bookId);

        if (existingUserBook is not null)
            return ValueOrResult.Failure("Book already added to user library.");

        var userBookResult = UserBook.Create(userId, bookId);
        if (!userBookResult.isSuccess)
            return ValueOrResult.Failure(userBookResult.ErrorMessage!);

        await _repository.AddUserBook(userId, bookId);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<UserBook>> GetUserBooksAsync(string userId)
    {
        return await _repository.GetAllUserBooks(userId);
    }

    public async Task<ValueOrResult> RemoveUserBookAsync(string userId, int bookId)
    {
        var userBookExists = await _repository.GetUserBookById(userId, bookId);
        if (userBookExists is null)
            return ValueOrResult.Failure("The user does not have such a book.");

        await _repository.Delete(userBookExists);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
