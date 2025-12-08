using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Books.Services;
using Librium.Domain.Common;
using Librium.Domain.DTOs.Books;

namespace Librium.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    private readonly IBookCategoryRepository _categoryRepo;

    public BookService(IBookRepository repository, IBookCategoryRepository category)
    {
        _bookRepo = repository;
        _categoryRepo = category;
    }

    public async Task<ValueOrResult<Guid>> CreateBookAsync(BookDto bookDto)
    {
        var existingBook = await _bookRepo.GetAllBooks();
        bool bookExists = existingBook.Any(b => b.Author == bookDto.Author!.Trim() && b.Title == bookDto.Title!.Trim());
        if (bookExists)
            return ValueOrResult<Guid>.Failure("A book with the same author and title already exsits.");

        var existingCategory = await _categoryRepo.GetAllBookCategories();
        var categoryExists = existingCategory.FirstOrDefault(c => c.Name.Trim() == bookDto.Category!.Trim());
        if (categoryExists is null)
            return ValueOrResult<Guid>.Failure($"Category {bookDto.Category} does not exists.");

        var bookResult = Book.Create(bookDto.Title!, bookDto.Author!, categoryExists.Id, bookDto.Content!, bookDto.PublishedYear);

        if (!bookResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(bookResult.ErrorMessage!);

        var book = bookResult.Value!;

        await _bookRepo.AddBook(book);
        await _bookRepo.SaveChanges();

        return ValueOrResult<Guid>.Success(book.Id);
    }

    public async Task<ValueOrResult> DeleteBookAsync(Guid bookId)
    {
        var book = await _bookRepo.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        _bookRepo.Delete(book);
        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await _bookRepo.GetAllBooks();

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

    public async Task<BookResponseDto> GetBookById(Guid bookId)
    {
        var book = await _bookRepo.GetBookById(bookId);

        return new BookResponseDto
        {
            Id = book!.Id,
            Title = book.Title,
            Author = book.Author,
            BookCategory = book.BookCategory.Name,
            Content = book.Content,
            PublishedYear = book.PublishedYear
        };
    }

    public async Task<ValueOrResult> UpdateBookAsync(Guid bookId, BookDto bookDto)
    {
        var existingBook = await _bookRepo.GetBookById(bookId);
        if (existingBook is null)
            return ValueOrResult.Failure("Book not found.");

        var existingCategory = await _categoryRepo.GetAllBookCategories();
        var categoryExists = existingCategory.FirstOrDefault(c => c.Name == bookDto.Category);
        if (categoryExists is null)
            return ValueOrResult.Failure("Category does not exist.");

        var updatedResult = existingBook.Update(
            bookDto.Title!,
            bookDto.Author!,
            bookDto.Content!,
            bookDto.PublishedYear,
            categoryExists
        );
        if (!updatedResult.IsSuccess)
            return ValueOrResult.Failure(updatedResult.ErrorMessage!);

        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }
}
