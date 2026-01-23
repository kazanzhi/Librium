using Librium.Application.Categories.DTOs;
using MediatR;

namespace Librium.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryResponseDto>;