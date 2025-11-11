using Librium.Domain.Users.Models;
using Librium.Domain.Users.Repositories;
using Librium.Domain.Users.Services;

namespace Librium.Application.Services;
public class UserBookService : IUserBookService
{
    private readonly IUserBookRepository _repo;
    public UserBookService(IUserBookRepository userBookRepository)
    {
        _repo = userBookRepository;
    }

    public async Task<int> AddUserBookAsync(string userId, int bookId)
    {
        return await _repo.AddUserBook(userId, bookId);
    }

    public async Task<List<UserBook>> GetUserBooksAsync(string userId)
    {
        return await _repo.GetUserBooks(userId);
    }

    public async Task<int> RemoveUserBookAsync(string userId, int bookId)
    {
        return await _repo.RemoveUserBook(userId, bookId);
    }
}
