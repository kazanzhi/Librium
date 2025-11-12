using Librium.Domain.Books.DTOs;
using Librium.Domain.Common;
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
    public Task<ValueOrResult<int>> AddBookCategoryAsync(BookCategoryDto categoryDto)
    {
        
    }

    public Task<ValueOrResult> DeleteBookCategoryAsync(int categoryId)
    {

    }

    public Task<List<BookCategory>> GetBookCategoriesAsync()
    {

    }

    public Task<ValueOrResult> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto)
    {

    }
}
