using Librium.Application.Books.DTOs;
using Librium.Application.Categories.DTOs;
using Librium.Domain.Books.Repositories;
using MediatR;

namespace Librium.Application.Books.Queries.GetAllBooks;

public sealed class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IReadOnlyList<BookResponseDto>>
{
    private readonly IBookRepository _repo;
    public GetAllBooksQueryHandler(IBookRepository repo)
    {
        _repo = repo;
    }
    public async Task<IReadOnlyList<BookResponseDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _repo.GetAllBooks(request.Search);

        return books.Select(book => new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Categories = book.Categories
                .Select(c => new CategoryResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList(),
            Content = book.Content,
            PublishedYear = book.PublishedYear
        }).ToList();
    }
}