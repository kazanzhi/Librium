using Librium.Application.Categories.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid CategoryId, CategoryDto Dto) : IRequest<ValueOrResult>;