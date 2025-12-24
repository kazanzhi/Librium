using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
using Librium.Domain.DTOs.BookCategories;
using Librium.Domain.Books.Models;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Books.Services;

namespace Librium.Application.Services;

public class BookCategoryService : IBookCategoryService
{
    private readonly IBookCategoryRepository _repository;

    public BookCategoryService(IBookCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ValueOrResult<Guid>> CreateBookCategoryAsync(BookCategoryDto categoryDto)
    {
        var name = categoryDto.Name!.Trim();
        var categoryExists = await _repository.GetByNameAsync(name);
        if (categoryExists is not null)
            return ValueOrResult<Guid>.Failure("This category already exists.");
       
        var categoryResult = BookCategory.Create(name);
        if (!categoryResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(categoryResult.ErrorMessage!);

        var category = categoryResult.Value;

        await _repository.AddBookCategory(category);
        await _repository.SaveChanges();

        return ValueOrResult<Guid>.Success(category.Id);
    }

    public async Task<ValueOrResult> DeleteBookCategoryAsync(Guid categoryId)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        _repository.Delete(category);
        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }

    public async Task<List<BookCategoryResponseDto>> GetAllBookCategoriesAsync()
    {
        var categories = await _repository.GetAllBookCategoriesAsync();

        return categories.Select(category => new BookCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }

    public async Task<BookCategoryResponseDto> GetBookCategoryById(Guid categoryId)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);

        return new BookCategoryResponseDto
        {
            Id = category!.Id,
            Name = category.Name
        };
    }

    public async Task<ValueOrResult> UpdateBookCategoryAsync(Guid categoryId, BookCategoryDto categoryDto)
    {
        var category = await _repository.GetBookCategoryByIdAsync(categoryId);
        if (category is null)
            return ValueOrResult.Failure("Category not found.");

        var updateResult = category.Update(categoryDto.Name!);
        if (!updateResult.IsSuccess)
            return ValueOrResult.Failure(updateResult.ErrorMessage!);

        await _repository.SaveChanges();

        return ValueOrResult.Success();
    }
}
