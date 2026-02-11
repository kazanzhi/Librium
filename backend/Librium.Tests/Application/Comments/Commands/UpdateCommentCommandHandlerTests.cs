using FluentAssertions;
using Librium.Application.Comments.Commands.UpdateComment;
using Librium.Application.Comments.DTOs;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Commands;
public class UpdateCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCommentExistsAndBelongToUser()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var dto = new CommentDto { Content = "Content" };
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new UpdateCommentCommandHandler(repoMock.Object);
        var command = new UpdateCommentCommand(comment.Id, userId, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        comment.Content.Should().Be(dto.Content);

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotExist()
    {
        //arrange
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CommentDto { Content = "Content" };

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment?)null);

        var handler = new UpdateCommentCommandHandler(repoMock.Object);
        var command = new UpdateCommentCommand(commentId, userId, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();

        repoMock
            .Verify(r => r.GetByIdAsync(commentId), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotBelongToUser()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var dto = new CommentDto { Content = "Content" };
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new UpdateCommentCommandHandler(repoMock.Object);
        var command = new UpdateCommentCommand(comment.Id, Guid.NewGuid(), dto);

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

    [Fact]
    public async Task Handle_ShouldFail_WhenDomainValidationFails()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var dto = new CommentDto { Content = "C" };
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new UpdateCommentCommandHandler(repoMock.Object);
        var command = new UpdateCommentCommand(comment.Id, userId, dto);

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