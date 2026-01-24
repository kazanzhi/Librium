using Librium.Application.Comments.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.UpdateComment;

public sealed record UpdateCommentCommand(Guid CommentId, Guid UserId, CommentDto Dto) : IRequest<ValueOrResult>;