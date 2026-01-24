using Librium.Application.Libraries.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Libraries.Commands.AddBookToUserLibrary;

public sealed class AddBookToLibraryCommandHandler : IRequestHandler<AddBookToLibraryCommand, ValueOrResult>
{
    private readonly IUserBookRepository _userBookRepo;
    public AddBookToLibraryCommandHandler(IUserBookRepository userBookRepo)
    {
        _userBookRepo = userBookRepo;
    }
    public async Task<ValueOrResult> Handle(AddBookToLibraryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _userBookRepo.ExistsAsync(request.UserId, request.BookId);
        if (exists)
            return ValueOrResult.Failure("Book already in library.");

        _userBookRepo.Add(request.UserId, request.BookId);

        await _userBookRepo.SaveChangesAsync();
    
        return ValueOrResult.Success();
    }
}