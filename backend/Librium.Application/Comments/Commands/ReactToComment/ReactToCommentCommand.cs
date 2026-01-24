using Librium.Domain.Comments.Enums;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.ReactToComment;

public sealed record ReactToCommentCommand(Guid CommentId, Guid UserId, ReactionType ReactionType) : IRequest<ValueOrResult>;