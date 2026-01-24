using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Libraries.Commands.AddBookToUserLibrary;

public sealed record AddBookToLibraryCommand(Guid UserId, Guid BookId) : IRequest<ValueOrResult>;