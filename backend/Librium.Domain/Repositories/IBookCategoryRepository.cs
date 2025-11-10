using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Repositories
{
    public interface IBookCategoryRepository
    {
        Task<List<BookCategory>> GetBookCategories();
        Task<BookCategory> GetBookCategory(int categoryId);
        Task<BookCategory> CreateBookCategory(BookCategoryDto categoryDto);
        Task<int> DeleteBookCategory(int categoryId);
        Task<int> UpdateBookCategory(int categoryId, BookCategoryDto categoryDto);
    }
}
