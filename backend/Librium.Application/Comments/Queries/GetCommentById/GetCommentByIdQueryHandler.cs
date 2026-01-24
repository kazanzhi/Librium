using Librium.Application.Comments.DTOs;
using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Queries.GetCommentById;

public sealed class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, ValueOrResult<CommentResponseDto>>
{
    private readonly ICommentRepository _commentRepo;
    public GetCommentByIdQueryHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<ValueOrResult<CommentResponseDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepo.GetByIdAsync(request.CommentId);
        if (comment is null)
            return ValueOrResult<CommentResponseDto>.Failure("Comment is not found.");

        var dto = new CommentResponseDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            TotalLikes = comment.TotalLikes,
            TotalDislikes = comment.TotalDislikes,
            IsEdited = comment.IsEdited,
        };

        return ValueOrResult<CommentResponseDto>.Success(dto);
    }
}
