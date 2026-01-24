using Librium.Application.Comments.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Queries.GetCommentById;

public sealed record GetCommentByIdQuery(Guid CommentId) : IRequest<ValueOrResult<CommentResponseDto>>;