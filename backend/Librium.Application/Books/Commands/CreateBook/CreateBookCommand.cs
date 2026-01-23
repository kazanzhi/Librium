using Librium.Application.Books.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.CreateBook;

public sealed record CreateBookCommand(BookDto Dto) : IRequest<ValueOrResult<Guid>>;