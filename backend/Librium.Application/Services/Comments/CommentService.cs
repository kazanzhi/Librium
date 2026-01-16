using Librium.Application.Abstractions.Services;
using Librium.Application.DTOs.Comments;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;

namespace Librium.Application.Services.Comments;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    public async Task<ValueOrResult<Guid>> Create(Guid userId, Guid bookId, CommentDto dto)
    {
        var commentResult = Comment.Create(userId, bookId, dto.Content, DateTime.UtcNow);
        if(!commentResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(commentResult.ErrorMessage);

        var comment = commentResult.Value!;
        _commentRepository.Add(comment);
        await _commentRepository.SaveChangesAsync();

        return ValueOrResult<Guid>.Success(comment.Id);
    }

    public async Task<ValueOrResult> Delete(Guid commentId, Guid userId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment is null)
            return ValueOrResult.Failure("Comment is not found.");

        if(comment.UserId != userId)
            return ValueOrResult.Failure("You are not allowed to modify this comment.");

        _commentRepository.Remove(comment);
        await _commentRepository.SaveChangesAsync();

        return ValueOrResult.Success();
    }

    public async Task<ValueOrResult<CommentResponseDto>> GetById(Guid commentId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment is null)
            return ValueOrResult<CommentResponseDto>.Failure("Comment is not found.");

        var dto = new CommentResponseDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            IsEdited = comment.IsEdited,
        };

        return ValueOrResult<CommentResponseDto>.Success(dto);
    }

    public async Task<IReadOnlyList<CommentResponseDto>> GetForBook(Guid bookId)
    {
        var comments = await _commentRepository.GetByBookIdAsync(bookId);

        return comments.Select(c => new CommentResponseDto
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            IsEdited = c.IsEdited
        }).ToList();
    }

    public async Task<ValueOrResult> Update(Guid commentId, Guid userId, CommentDto dto)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment is null)
            return ValueOrResult.Failure("Comment is not found.");

        if(comment.UserId != userId)
            return ValueOrResult.Failure("You are not allowed to modify this comment.");

        var updateResult = comment.Update(dto.Content);
        if (!updateResult.IsSuccess)
            return ValueOrResult.Failure(updateResult.ErrorMessage);

        await _commentRepository.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
