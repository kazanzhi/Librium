using Librium.Domain.Comments.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Comments.Commands.UpdateComment;

public sealed class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, ValueOrResult>
{
    private readonly ICommentRepository _commentRepo;
    public UpdateCommentCommandHandler(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<ValueOrResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepo.GetByIdAsync(request.CommentId);
        if (comment is null)
            return ValueOrResult.Failure("Comment is not found.");

        if (comment.UserId != request.UserId)
            return ValueOrResult.Failure("You are not allowed to modify this comment.");

        var updateResult = comment.Update(request.Dto.Content);
        if (!updateResult.IsSuccess)
            return ValueOrResult.Failure(updateResult.ErrorMessage);

        await _commentRepo.SaveChangesAsync();

        return ValueOrResult.Success();
    }
}
