using FluentAssertions;
using Librium.Application.Comments.Commands.ReactToComment;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Enums;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Commands;

public class ReactToCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCommentExists()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var resctionType = ReactionType.Like;
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new ReactToCommentCommandHandler(repoMock.Object);
        var command = new ReactToCommentCommand(comment.Id, userId, resctionType);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotExist()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var resctionType = ReactionType.Like;
        var commentId = Guid.NewGuid();

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment?)null);

        var handler = new ReactToCommentCommandHandler(repoMock.Object);
        var command = new ReactToCommentCommand(commentId, userId, resctionType);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.GetByIdAsync(commentId), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDomainValidationFails()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var resctionType = ReactionType.Like;
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new ReactToCommentCommandHandler(repoMock.Object);
        var command = new ReactToCommentCommand(comment.Id, Guid.Empty, resctionType);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}