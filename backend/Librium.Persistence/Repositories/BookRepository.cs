using Librium.Domain.Books.Models;
using Librium.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibriumDbContext _context;
    public BookRepository(LibriumDbContext context)
    {
        _context = context;
    }
    public async Task<Book> AddBook(Book book)
    {
        var result = await _context.Books.AddAsync(book);
        return result.Entity;
    }

    public async Task Delete(Book entity)
    {
        _context.Books.Remove(entity);
    }

    public async Task<List<Book>> GetAllBooks()
    {
        return await _context.Books
            .Include(c => c.BookCategory)
            .ToListAsync();
    }

    public async Task<Book?> GetBookById(int bookId)
    {
        return await _context.Books
            .Include(c => c.BookCategory)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
