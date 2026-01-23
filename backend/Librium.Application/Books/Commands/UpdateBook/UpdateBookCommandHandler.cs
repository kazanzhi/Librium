using Librium.Application.Books.DTOs;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, ValueOrResult>
{
    private readonly IBookRepository _repo;

    public UpdateBookCommandHandler(IBookRepository repo)
    {
        _repo = repo;
    }
    public async Task<ValueOrResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _repo.GetBookById(request.Id);
        if (existingBook is null)
            return ValueOrResult.Failure("Book not found.");

        var updatedResult = existingBook.Update(
            request.Dto.Title,
            request.Dto.Author,
            request.Dto.Content,
            request.Dto.PublishedYear
        );

        if (!updatedResult.IsSuccess)
            return ValueOrResult.Failure(updatedResult.ErrorMessage!);

        await _repo.SaveChanges();

        return ValueOrResult.Success();
    }
}
