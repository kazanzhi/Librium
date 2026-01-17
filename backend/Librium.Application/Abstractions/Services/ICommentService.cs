using Librium.Application.DTOs.Comments;
using Librium.Domain.Comments;
using Librium.Domain.Common;

namespace Librium.Application.Abstractions.Services;
public interface ICommentService
{
    Task<IReadOnlyList<CommentResponseDto>> GetForBook(Guid bookId);
    Task<ValueOrResult<CommentResponseDto>> GetById(Guid commentId);
    Task<ValueOrResult<Guid>> Create(Guid userId, Guid bookId, CommentDto dto);
    Task<ValueOrResult> Update(Guid commentId, Guid userId, CommentDto dto);
    Task<ValueOrResult> Delete(Guid commentId, Guid userId);
}
