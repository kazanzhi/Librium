using FluentAssertions;
using Librium.Application.Comments.Commands.CreateComment;
using Librium.Application.Comments.DTOs;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Commands;
public class CreateCommentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var dto = new CommentDto { Content = "Content" };

        var repoMock = new Mock<ICommentRepository>();

        var handle = new CreateCommentCommandHandler(repoMock.Object);
        var command = new CreateCommentCommand(userId, bookId, dto);

        //act
        var result = await handle.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        repoMock
            .Verify(r => r.Add(It.Is<Comment>(
                c => c.Content == dto.Content 
                && c.UserId == userId 
                && c.BookId == bookId)), Times.Once
            );

        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDomainValidationFails()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var dto = new CommentDto { Content = "C" };

        var repoMock = new Mock<ICommentRepository>();

        var handle = new CreateCommentCommandHandler(repoMock.Object);
        var command = new CreateCommentCommand(userId, bookId, dto);

        //act
        var result = await handle.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);

        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}