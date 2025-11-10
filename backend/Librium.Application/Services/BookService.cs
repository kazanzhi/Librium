using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;

namespace Librium.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository bookRepository)
        {
            _repo = bookRepository;
        }

        public async Task<Book> CreateBookAsync(BookDto bookDto)
        {
            return await _repo.CreateBook(bookDto);
        }

        public async Task<int> DeleteBookAsync(int bookId)
        {
            return await _repo.DeleteBook(bookId);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _repo.GetBook(bookId);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _repo.GetBooks();
        }

        public async Task<int> UpdateBookAsync(int bookId, BookDto bookDto)
        {
            return await _repo.UpdateBook(bookId, bookDto);
        }
    }
}
