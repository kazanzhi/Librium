using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Common;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;

namespace Librium.Application.Services;
public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }
    public async Task<ValueOrResult<int>> AddBookAsync(BookDto bookDto)
    {
        
    }

    public Task<ValueOrResult> DeleteBookAsync(int bookId)
    {
        
    }

    public async Task<List<Book>> GetBooksAsync()
    {

    }

    public Task<ValueOrResult> UpdateBookAsync(int bookId, BookDto bookDto)
    {
        
    }
}
