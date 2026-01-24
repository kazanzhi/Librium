using Librium.Domain.Books.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, ValueOrResult>
{
    private readonly IBookRepository _repo;
    public DeleteBookCommandHandler(IBookRepository repo)
    {
        _repo = repo;
    }
    public async Task<ValueOrResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _repo.GetBookById(request.Id);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        _repo.Delete(book);
        await _repo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
