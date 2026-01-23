using Librium.Application.Categories.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(CategoryDto Dto) : IRequest<ValueOrResult<Guid>>;