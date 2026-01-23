using Librium.Application.Books.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand(Guid Id, BookDto Dto) : IRequest<ValueOrResult>;