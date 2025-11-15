using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.Books;
using Librium.Application.Interfaces;
using Librium.Domain.Books.Models;
using Librium.Domain.Common;
using Librium.Domain.Repositories;

namespace Librium.Application.Services;
public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IBookCategoryRepository _category;
    public BookService(IBookRepository repository, IBookCategoryRepository category)
    {
        _repository = repository;
        _category = category;
    }

    public async Task<ValueOrResult<int>> AddBookAsync(BookDto bookDto)
    {
        var existingBok = await _repository.GetAllBooks();
        bool bookExists = existingBok.Any(b => b.Author == bookDto.Author.Trim() && b.Title == bookDto.Title.Trim());
        if (bookExists)
            return ValueOrResult<int>.Failure("A book with the same author and title already exsits.");

        var existingCategory = await _category.GetAllBookCategories();
        var categoryExists = existingCategory.FirstOrDefault(c => c.Name.Trim() == bookDto.Category.Trim());
        if (categoryExists is null)
            return ValueOrResult<int>.Failure($"Category {bookDto.Category} does not exists.");

        var bookResult = Book.Create(bookDto.Title, bookDto.Author, bookDto.Category, bookDto.Content, bookDto.PublishedYear);

        if (!bookResult.isSuccess)
            return ValueOrResult<int>.Failure(bookResult.ErrorMessage!);

        Book book = bookResult.Value;
        if (book is null)
            return ValueOrResult<int>.Failure("Something went wrong.");

        bookResult.Value.BookCategory = categoryExists;
        
        await _repository.AddBook(book);
        await _repository.SaveChanges();

        return ValueOrResult<int>.Success(book.Id);
    }

    public async Task<ValueOrResult> DeleteBookAsync(int bookId)
    {
        var book = await _repository.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        await _repository.Delete(book);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await _repository.GetAllBooks();

        return books.Select(book => new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            BookCategory = book.BookCategory.Name,
            Content = book.Content,
            PublishedYear = book.PublishedYear
        }).ToList();
    }

    public async Task<BookResponseDto> GetBookById(int bookId)
    {
        var book = await _repository.GetBookById(bookId);

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            BookCategory = book.BookCategory.Name,
            Content = book.Content,
            PublishedYear = book.PublishedYear
        };
    }

    public async Task<ValueOrResult> UpdateBookAsync(int bookId, BookDto bookDto)
    {
        var existingBook = await _repository.GetBookById(bookId);
        if (existingBook is null)
            return ValueOrResult.Failure("Book not found.");

        var existingCategory = await _category.GetAllBookCategories();
        var categoryExists = existingCategory.FirstOrDefault(c => c.Name == bookDto.Category);
        if (categoryExists is null)
            return ValueOrResult.Failure("Category does not exist.");

        existingBook.Title = bookDto.Title.Trim();
        existingBook.Author = bookDto.Author.Trim();
        existingBook.PublishedYear = bookDto.PublishedYear;
        existingBook.BookCategory = categoryExists;

        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
