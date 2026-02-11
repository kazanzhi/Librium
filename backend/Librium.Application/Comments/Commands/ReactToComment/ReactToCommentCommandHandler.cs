using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.ReactToComment;

public sealed class ReactToCommentCommandHandler : IRequestHandler<ReactToCommentCommand, ValueOrResult>
{
    private readonly ICommentRepository _commentRepo;
    public ReactToCommentCommandHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<ValueOrResult> Handle(ReactToCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepo.GetByIdAsync(request.CommentId);
        if (comment is null)
            return ValueOrResult.Failure("Comment is not found.");

        var reactionResult = comment.React(request.UserId, request.ReactionType);
        if (!reactionResult.IsSuccess)
            return ValueOrResult.Failure(reactionResult.ErrorMessage);

        await _commentRepo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
