using Librium.Application.Comments.DTOs;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.CreateComment;

public sealed record CreateCommentCommand(Guid UserId, Guid BookId, CommentDto Dto) : IRequest<ValueOrResult<Guid>>;