using Librium.Application.Comments.DTOs;
using Librium.Domain.Comments.Repositories;
using MediatR;

namespace Librium.Application.Comments.Queries.GetCommentsForBook;

public sealed class GetCommentsForBookQueryHandler : IRequestHandler<GetCommentsForBookQuery, IReadOnlyList<CommentResponseDto>>
{
    private readonly ICommentRepository _commentRepo;
    public GetCommentsForBookQueryHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<IReadOnlyList<CommentResponseDto>> Handle(
        GetCommentsForBookQuery request, 
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepo.GetByBookIdAsync(request.BookId);

        return comments.Select(c => new CommentResponseDto
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            TotalLikes = c.TotalLikes,
            TotalDislikes = c.TotalDislikes,
            IsEdited = c.IsEdited
        }).ToList();
    }
}
