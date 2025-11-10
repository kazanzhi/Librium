using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(int bookId);

        Task<Book> CreateBook(BookDto bookDto);
        Task<int> DeleteBook(int bookId);
        Task<int> UpdateBook(int bookId, BookDto bookDto);
    }
}
