using Librium.Application.Books.DTOs;
using MediatR;

namespace Librium.Application.Libraries.Queries.GetUserLibrary;

public sealed record GetUserLibraryQuery(Guid UserId) : IRequest<IReadOnlyList<BookResponseDto>>;