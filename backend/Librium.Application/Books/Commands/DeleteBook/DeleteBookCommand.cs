using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.DeleteBook;

public sealed record DeleteBookCommand(Guid Id) : IRequest<ValueOrResult>;