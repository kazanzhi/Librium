using Librium.Application.Books.DTOs;
using MediatR;

namespace Librium.Application.Books.Queries.GetAllBooks;

public sealed record GetAllBooksQuery(string? Search) : IRequest<IReadOnlyList<BookResponseDto>>;