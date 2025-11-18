using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.DTOs.BookCategories;
using Librium.Domain.Entities.Books;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;

namespace Librium.Application.Services;
public class BookCategoryService : IBookCategoryService
{
    private readonly IBookCategoryRepository _repository;
    public BookCategoryService(IBookCategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<ValueOrResult<Guid>> AddBookCategoryAsync(BookCategoryDto categoryDto)
    {
        var categoryResult = BookCategory.Create(categoryDto.Name);
        if (!categoryResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(categoryResult.ErrorMessage!);

        BookCategory? category = categoryResult.Value;
        if (category is null)
            return ValueOrResult<Guid>.Failure("Something went wrong.");

        await _repository.AddBookCategory(category);
        await _repository.SaveChanges();

        return ValueOrResult<Guid>.Success(category.Id);
    }

    public async Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId)
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

    public async Task<BookCategoryResponseDto> GetBookCategoryById(Guid categoryId)
    {
        var category =  await _repository.GetBookCategoryById(categoryId);

        return new BookCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, BookCategoryDto categoryDto)
    {
        var category = await _repository.GetBookCategoryById(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        category.Name = categoryDto.Name.Trim();

        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
