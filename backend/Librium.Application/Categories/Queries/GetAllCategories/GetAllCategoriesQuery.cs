using Librium.Application.Categories.DTOs;
using MediatR;

namespace Librium.Application.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryResponseDto>>;