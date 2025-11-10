using Librium.Application.Common.Exceptions;
using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;

namespace Librium.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCategoryRepository _categoryRepository;
        public BookService(IBookRepository bookRepository, IBookCategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Book> CreateBookAsync(BookDto bookDto)
        {
            var bookExists = await _bookRepository.GetBooks();
            if (bookExists.Any(b => b.Author == bookDto.Author && b.Title == bookDto.Title))
                throw new BusinessRuleViolationException("Book with this title and author already exists.");

            var categoryExist = await _categoryRepository.GetBookCategories();
            if (!categoryExist.Any(c => c.Name == bookDto.Category))
                throw new BusinessRuleViolationException($"Category {bookDto.Category} does not exist.");

            
            return await _bookRepository.CreateBook(bookDto);
        }

        public async Task<int> DeleteBookAsync(int bookId)
        {
            var bookExist = await _bookRepository.GetBook(bookId);
            if (bookExist is null)
                throw new NotFoundException(nameof(Book), bookId);

            return await _bookRepository.DeleteBook(bookId);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var bookExist = await _bookRepository.GetBook(bookId);
            if (bookExist is null)
                throw new NotFoundException(nameof(Book), bookId);

            return await _bookRepository.GetBook(bookId);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _bookRepository.GetBooks();
        }

        public async Task<int> UpdateBookAsync(int bookId, BookDto bookDto)
        {
            var bookExist = await _bookRepository.GetBook(bookId);
            if (bookExist is null)
                throw new NotFoundException(nameof(Book), bookId);

            var categoryExist = await _categoryRepository.GetBookCategories();
            if (!categoryExist.Any(c => c.Name == bookDto.Category))
                throw new BusinessRuleViolationException($"Category {bookDto.Category} does not exist.");

            return await _bookRepository.UpdateBook(bookId, bookDto);
        }
    }
}
