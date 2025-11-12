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

    public async Task<int> AddUserBookAsync(string userId, int bookId)
    {

    }

    public async Task<List<UserBook>> GetUserBooksAsync(string userId)
    {

    }

    public async Task<int> RemoveUserBookAsync(string userId, int bookId)
    {

    }
}
