using Librium.Application.Comments.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Queries.GetCommentsForBook;

public sealed record GetCommentsForBookQuery(Guid BookId) : IRequest<IReadOnlyList<CommentResponseDto>>;