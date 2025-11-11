using Librium.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Repositories;
public interface IUserBookRepository
{
    Task<List<UserBook>> GetUserBooks(string userId);
    Task<int> AddUserBook(string userId, int bookId);
    Task<int> RemoveUserBook(string userId, int bookId);
}
