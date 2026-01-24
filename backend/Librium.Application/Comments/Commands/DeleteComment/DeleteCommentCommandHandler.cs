using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.DeleteComment;

public sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, ValueOrResult>
{
    private readonly ICommentRepository _commentRepo;
    public DeleteCommentCommandHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<ValueOrResult> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepo.GetByIdAsync(request.CommentId);
        if (comment is null)
            return ValueOrResult.Failure("Comment is not found.");

        if (comment.UserId != request.UserId)
            return ValueOrResult.Failure("You are not allowed to modify this comment.");

        _commentRepo.Delete(comment);
        await _commentRepo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
