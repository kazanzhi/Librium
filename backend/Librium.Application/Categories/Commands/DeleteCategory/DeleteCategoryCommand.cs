using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid CategoryId) : IRequest<ValueOrResult>;