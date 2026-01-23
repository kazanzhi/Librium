using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.AddCategoryToBook;

public sealed record AddCategoryToBookCommand(Guid BookId, Guid CategoryId) : IRequest<ValueOrResult>;