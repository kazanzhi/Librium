using FluentAssertions;
using Librium.Application.Libraries.Commands.RemoveBookFromUserLibrary;
using Librium.Application.Libraries.Repositories;
using Moq;

namespace Librium.Tests.Application.Libraries.Commands;
public class RemoveBookFromUserLibraryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenBookIsInLibrary()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.ExistsAsync(userId, bookId)).ReturnsAsync(true);

        var handler = new RemoveBookFromUserLibraryCommandHandler(userBookRepo.Object);
        var command = new RemoveBookFromUserLibraryCommand(userId, bookId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        userBookRepo
            .Verify(r => r.ExistsAsync(userId, bookId), Times.Once);
        userBookRepo
            .Verify(r => r.Remove(userId, bookId), Times.Once);
        userBookRepo
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookIsNotInLibrary()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.ExistsAsync(userId, bookId)).ReturnsAsync(false);

        var handler = new RemoveBookFromUserLibraryCommandHandler(userBookRepo.Object);
        var command = new RemoveBookFromUserLibraryCommand(userId, bookId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        userBookRepo
            .Verify(r => r.Remove(userId, bookId), Times.Never);
        userBookRepo
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}