using Librium.Domain.Common.Repositories;
using Librium.Domain.Books.Models;

namespace Librium.Domain.Books.Repositories;

public interface IBookCategoryRepository : IBaseRepository<BookCategory>
{
    Task<BookCategory?> GetBookCategoryById(Guid categoryId);
    Task<List<BookCategory>> GetAllBookCategories();
    Task<BookCategory> AddBookCategory(BookCategory category);
}
