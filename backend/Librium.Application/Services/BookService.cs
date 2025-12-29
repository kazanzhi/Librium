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
    private readonly ICategoryRepository _categoryRepo;

    public BookService(IBookRepository repository, ICategoryRepository category)
    {
        _bookRepo = repository;
        _categoryRepo = category;
    }

    public async Task<ValueOrResult> AddCategoryToBook(Guid bookId, string categoryName)
    {
        var book = await _bookRepo.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        var category = await _categoryRepo.GetByNameAsync(categoryName);
        if (category is null)
            return ValueOrResult.Failure($"Category {categoryName} does not exists.");

        var addResult = book.AddCategory(category);
        if (!addResult.IsSuccess)
            return ValueOrResult.Failure(addResult.ErrorMessage!);

        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult<Guid>> CreateBookAsync(BookDto bookDto)
    {
        var bookExists = await _bookRepo.ExistBookAsync(bookDto.Author, bookDto.Title);
        if (bookExists)
            return ValueOrResult<Guid>.Failure("A book with the same author and title already exsits.");
        
        var bookResult = Book.Create(bookDto.Title!, bookDto.Author!, bookDto.Content!, bookDto.PublishedYear);

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
            Categories = book.Categories
                .Select(c => c.Name)
                .ToList(),
            Content = book.Content,
            PublishedYear = book.PublishedYear
        }).ToList();
    }

    public async Task<ValueOrResult<BookResponseDto>> GetBookById(Guid bookId)
    {
        var book = await _bookRepo.GetBookById(bookId);
        if (book is null)
            return ValueOrResult<BookResponseDto>.Failure("Book not found.");

        var dto = new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Categories = book.Categories
                .Select(c => c.Name)
                .ToList(),
            Content = book.Content,
            PublishedYear = book.PublishedYear
        };

        return ValueOrResult<BookResponseDto>.Success(dto);
    }

    public async Task<ValueOrResult> RemoveCategoryFromBook(Guid bookId, string categoryName)
    {
        var book = await _bookRepo.GetBookById(bookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        var category = await _categoryRepo.GetByNameAsync(categoryName);
        if (category is null)
            return ValueOrResult.Failure($"Category {categoryName} does not exists.");

        var addResult = book.RemoveCategory(category);
        if (!addResult.IsSuccess)
            return ValueOrResult.Failure(addResult.ErrorMessage!);

        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult> UpdateBookAsync(Guid bookId, BookDto bookDto)
    {
        var existingBook = await _bookRepo.GetBookById(bookId);
        if (existingBook is null)
            return ValueOrResult.Failure("Book not found.");

        var updatedResult = existingBook.Update(
            bookDto.Title!,
            bookDto.Author!,
            bookDto.Content!,
            bookDto.PublishedYear
        );

        if (!updatedResult.IsSuccess)
            return ValueOrResult.Failure(updatedResult.ErrorMessage!);

        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }
}
