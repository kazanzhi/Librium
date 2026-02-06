using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ValueOrResult<Guid>>
{
    private readonly IBookRepository _repo;
    public CreateBookCommandHandler(IBookRepository repo)
    {
        _repo = repo;
    }
    public async Task<ValueOrResult<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookExists = await _repo.ExistBookAsync(request.Dto.Author, request.Dto.Title);
        if (bookExists)
            return ValueOrResult<Guid>.Failure("A book with the same author and title already exsits.");

        var bookResult = Book.Create(request.Dto.Title, request.Dto.Author, request.Dto.Content, request.Dto.PublishedYear);

        if (!bookResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(bookResult.ErrorMessage!);

        var book = bookResult.Value!;

        _repo.Add(book);
        await _repo.SaveChangesAsync();

        return ValueOrResult<Guid>.Success(book.Id);
    }
}
