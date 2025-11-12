using Librium.Domain.Books.DTOs;
using Librium.Domain.Common.Repositories;
using Librium.Domain.Entities.Books;

namespace Librium.Domain.Repositories;
public interface IBookCategoryRepository : IBaseRepository
{
    Task<BookCategory?> GetBookCategoryById(int categoryId);
    Task<BookCategory> AddBookCategory(BookCategoryDto categoryDto);
}
