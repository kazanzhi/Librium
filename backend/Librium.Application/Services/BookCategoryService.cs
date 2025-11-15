using Librium.Application.Books.DTOs;
using Librium.Application.DTOs.BookCategories;
using Librium.Application.DTOs.Books;
using Librium.Application.Interfaces;
using Librium.Domain.Common;
using Librium.Domain.Entities.Books;
using Librium.Domain.Repositories;

namespace Librium.Application.Services;
public class BookCategoryService : IBookCategoryService
{
    private readonly IBookCategoryRepository _repository;
    public BookCategoryService(IBookCategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<ValueOrResult<int>> AddBookCategoryAsync(BookCategoryDto categoryDto)
    {
        var categoryResult = BookCategory.Create(categoryDto.Name);
        if (!categoryResult.isSuccess)
            return ValueOrResult<int>.Failure(categoryResult.ErrorMessage!);

        BookCategory? category = categoryResult.Value;
        if (category is null)
            return ValueOrResult<int>.Failure("Something went wrong.");

        await _repository.AddBookCategory(category);
        await _repository.SaveChanges();

        return ValueOrResult<int>.Success(category.Id);
    }

    public async Task<ValueOrResult> DeleteBookCategoryAsync(int categoryId)
    {
        var book = await _repository.GetBookCategoryById(categoryId);
        if (book is null)
            return ValueOrResult.Failure("Category not found.");

        await _repository.Delete(book);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<BookCategoryResponseDto>> GetAllBookCategoriesAsync()
    {
        var categories = await _repository.GetAllBookCategories();

        return categories.Select(category => new BookCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }

    public async Task<BookCategoryResponseDto> GetBookCategoryById(int categoryId)
    {
        var category =  await _repository.GetBookCategoryById(categoryId);

        return new BookCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<ValueOrResult> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto)
    {
        var category = await _repository.GetBookCategoryById(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        category.Name = categoryDto.Name.Trim();

        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
