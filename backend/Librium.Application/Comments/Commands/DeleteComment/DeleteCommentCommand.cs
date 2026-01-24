using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.DeleteComment;

public sealed record DeleteCommentCommand(Guid CommentId, Guid UserId) : IRequest<ValueOrResult>;