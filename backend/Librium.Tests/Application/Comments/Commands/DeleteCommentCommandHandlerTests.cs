using FluentAssertions;
using Librium.Application.Comments.Commands.DeleteComment;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Commands;
public class DeleteCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenUserOwnsComment()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new DeleteCommentCommandHandler(repoMock.Object);
        var command = new DeleteCommentCommand(comment.Id, userId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
        repoMock
            .Verify(r => r.Delete(comment), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotExist()
    {
        //arrange
        var userId = Guid.NewGuid();
        var commentId = Guid.NewGuid();

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment?)null);

        var handler = new DeleteCommentCommandHandler(repoMock.Object);
        var command = new DeleteCommentCommand(commentId, userId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.GetByIdAsync(commentId), Times.Once);
        repoMock
            .Verify(r => r.Delete(It.IsAny<Comment>()), Times.Never);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);

    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotBelongToUser()
    {
        //arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var comment = Comment.Create(userId1, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new DeleteCommentCommandHandler(repoMock.Object);
        var command = new DeleteCommentCommand(comment.Id, userId2);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
        repoMock
            .Verify(r => r.Delete(It.IsAny<Comment>()), Times.Never);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}