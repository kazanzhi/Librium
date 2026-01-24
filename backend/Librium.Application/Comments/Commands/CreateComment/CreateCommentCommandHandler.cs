using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ValueOrResult<Guid>>
{
    private readonly ICommentRepository _commentRepo;
    public CreateCommentCommandHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<ValueOrResult<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var commentResult = Comment.Create(request.UserId, request.BookId, request.Dto.Content, DateTime.UtcNow);
        if (!commentResult.IsSuccess)
            return ValueOrResult<Guid>.Failure(commentResult.ErrorMessage);

        var comment = commentResult.Value!;
        _commentRepo.Add(comment);
        await _commentRepo.SaveChangesAsync();

        return ValueOrResult<Guid>.Success(comment.Id);
    }
}
