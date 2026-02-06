using FluentAssertions;
using Librium.Application.Books.Commands.DeleteBook;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Commands;

public class DeleteBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenBookExists()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new DeleteBookCommandHandler(repoMock.Object);
        var command = new DeleteBookCommand(book.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        repoMock
            .Verify(r => r.Delete(book), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookDoesNotExist()
    {
        //arrange
        var bookId = Guid.NewGuid();

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        var handler = new DeleteBookCommandHandler(repoMock.Object);
        var command = new DeleteBookCommand(bookId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.GetBookById(bookId), Times.Once);
        repoMock
            .Verify(r => r.Delete(It.IsAny<Book>()), Times.Never);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}