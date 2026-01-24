using Librium.Application.Books.DTOs;
using Librium.Application.Categories.DTOs;
using Librium.Application.Libraries.Repositories;
using MediatR;

namespace Librium.Application.Libraries.Queries.GetUserLibrary;

public sealed class GetUserLibraryQueryHandler : IRequestHandler<GetUserLibraryQuery, IReadOnlyList<BookResponseDto>>
{
    private readonly IUserBookRepository _userBookRepo;
    public GetUserLibraryQueryHandler(IUserBookRepository userBookRepo)
    {
        _userBookRepo = userBookRepo;
    }
    public async Task<IReadOnlyList<BookResponseDto>> Handle(GetUserLibraryQuery request, CancellationToken cancellationToken)
    {
        var library = await _userBookRepo.GetBooksByUserIdAsync(request.UserId);

        return library.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Content = b.Content,
            PublishedYear = b.PublishedYear,
            Categories = b.Categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList()
        }).ToList();
    }
}
