using Librium.Application.Categories.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.RemoveCategoryFromBook;

public sealed record RemoveCategoryFromBookCommand(Guid BookId, Guid CategoryId) : IRequest<ValueOrResult>;