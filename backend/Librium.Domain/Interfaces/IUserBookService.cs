using Librium.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Interfaces
{
    public interface IUserBookService
    {
        Task<List<UserBook>> GetUserBooksAsync(string userId);
        Task<int> AddUserBookAsync(string userId, int bookId);
        Task<int> RemoveUserBookAsync(string userId, int bookId);
    }
}
