

using Librium.Domain.Dtos;
using Librium.Domain.Entities.Books;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;

namespace Librium.Application.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _repo;
        public BookCategoryService(IBookCategoryRepository bookCategoryRepository)
        {
            _repo = bookCategoryRepository;
        }

        public async Task<BookCategory> CreateBookCategoryAsync(BookCategoryDto categoryDto)
        {
            return await _repo.CreateBookCategory(categoryDto);
        }

        public async Task<int> DeleteBookCategoryAsync(int categoryId)
        {
            return await _repo.DeleteBookCategory(categoryId);
        }

        public async Task<List<BookCategory>> GetBookCategoriesAsync()
        {
            return await _repo.GetBookCategories();
        }

        public async Task<BookCategory> GetBookCategoryByIdAsync(int categoryId)
        {
            return await _repo.GetBookCategory(categoryId);
        }

        public async Task<int> UpdateBookCategoryAsync(int categoryId, BookCategoryDto categoryDto)
        {
            return await _repo.UpdateBookCategory(categoryId, categoryDto);
        }
    }
}
