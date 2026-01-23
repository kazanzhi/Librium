using Librium.Application.Books.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid Id) : IRequest<ValueOrResult<BookResponseDto>>;