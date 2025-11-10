using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);

        Task<Book> CreateBookAsync(BookDto bookDto);
        Task<int> DeleteBookAsync(int bookId);
        Task<int> UpdateBookAsync(int bookId, BookDto bookDto);
    }
}
