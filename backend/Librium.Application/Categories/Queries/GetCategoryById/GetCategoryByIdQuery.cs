using Librium.Application.Categories.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<ValueOrResult<CategoryResponseDto>>;