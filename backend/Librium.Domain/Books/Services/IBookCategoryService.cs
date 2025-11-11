using Librium.Domain.Books.Dtos;
using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Interfaces;
public interface IBookCategoryService
{
    Task<List<BookCategory>> GetBookCategoriesAsync();
    Task<BookCategory> GetBookCategoryByIdAsync(int categoryId);
    Task<BookCategory> CreateBookCategoryAsync(BookCategoryDto categoryDto);
    Task<int> DeleteBookCategoryAsync(int categoryId);
    Task<int> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto);
}
