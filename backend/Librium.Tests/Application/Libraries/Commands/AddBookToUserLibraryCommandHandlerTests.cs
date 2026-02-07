using FluentAssertions;
using Librium.Application.Libraries.Commands.AddBookToUserLibrary;
using Librium.Application.Libraries.Repositories;
using Moq;

namespace Librium.Tests.Application.Libraries.Commands;
public class AddBookToUserLibraryCommandHandlerTests
{
    [Fact]
    public async Task Handler_ShouldSuceed_WhenBookIsNotInLibrary()
    {
        //arrage
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.ExistsAsync(userId, bookId)).ReturnsAsync(false);

        var handler = new AddBookToLibraryCommandHandler(userBookRepo.Object);
        var command = new AddBookToLibraryCommand(userId, bookId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        userBookRepo
            .Verify(r => r.ExistsAsync(userId, bookId), Times.Once);
        userBookRepo
            .Verify(r => r.Add(userId, bookId), Times.Once);
        userBookRepo
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handler_ShouldFail_WhenBookIsInLibrary()
    {
        //arrage
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.ExistsAsync(userId, bookId)).ReturnsAsync(true);

        var handler = new AddBookToLibraryCommandHandler(userBookRepo.Object);
        var command = new AddBookToLibraryCommand(userId, bookId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        userBookRepo
            .Verify(r => r.Add(userId, bookId), Times.Never);
        userBookRepo
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}