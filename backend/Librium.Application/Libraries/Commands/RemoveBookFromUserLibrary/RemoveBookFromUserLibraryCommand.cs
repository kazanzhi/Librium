using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Libraries.Commands.RemoveBookFromUserLibrary;

public sealed record RemoveBookFromUserLibraryCommand(Guid UserId, Guid BookId) : IRequest<ValueOrResult>;