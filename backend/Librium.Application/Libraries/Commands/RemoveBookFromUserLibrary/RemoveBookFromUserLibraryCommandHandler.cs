using Librium.Application.Libraries.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Libraries.Commands.RemoveBookFromUserLibrary;

public sealed class RemoveBookFromUserLibraryCommandHandler : IRequestHandler<RemoveBookFromUserLibraryCommand, ValueOrResult>
{
    private readonly IUserBookRepository _userBookRepo;
    public RemoveBookFromUserLibraryCommandHandler(IUserBookRepository userBookRepo)
    {
        _userBookRepo = userBookRepo;
    }
    public async Task<ValueOrResult> Handle(RemoveBookFromUserLibraryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _userBookRepo
            .ExistsAsync(request.UserId, request.BookId);
        if (!exists)
            return ValueOrResult.Failure("Book is not in user's library.");

        await _userBookRepo.Remove(request.UserId, request.BookId);

        await _userBookRepo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
