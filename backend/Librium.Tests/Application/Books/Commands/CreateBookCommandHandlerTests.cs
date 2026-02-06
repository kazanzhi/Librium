using FluentAssertions;
using Librium.Application.Books.Commands.CreateBook;
using Librium.Application.Books.DTOs;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Commands;

public class CreateBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenBookIsNew()
    {
        //arrange
        var bookDto = new BookDto
        {
            Author = "Author",
            Title = "Title",
            Content = "Content",
            PublishedYear = 2000
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.ExistBookAsync(bookDto.Author, bookDto.Title)).ReturnsAsync(false);

        var handler = new CreateBookCommandHandler(repoMock.Object);
        var command = new CreateBookCommand(bookDto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        repoMock
            .Verify(r => r.ExistBookAsync(bookDto.Author, bookDto.Title), Times.Once);
        repoMock
            .Verify(r => r.Add(It.Is<Book>(
                c => c.Author == bookDto.Author
                && c.Title == bookDto.Title
                && c.Content == bookDto.Content
                && c.PublishedYear == bookDto.PublishedYear)), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenTheSameBookAlreadyExists()
    {
        //arrange
        var bookDto = new BookDto
        {
            Author = "Author",
            Title = "Title",
            Content = "Content",
            PublishedYear = 2000
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.ExistBookAsync(bookDto.Author, bookDto.Title)).ReturnsAsync(true);

        var handler = new CreateBookCommandHandler(repoMock.Object);
        var command = new CreateBookCommand(bookDto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.ExistBookAsync(bookDto.Author, bookDto.Title), Times.Once);
        repoMock
            .Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDomainValidationFails()
    {
        //arrange
        var bookDto = new BookDto
        {
            Author = "",
            Title = "Title",
            Content = "Content",
            PublishedYear = 2000
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.ExistBookAsync(bookDto.Author, bookDto.Title)).ReturnsAsync(false);

        var handler = new CreateBookCommandHandler(repoMock.Object);
        var command = new CreateBookCommand(bookDto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.ExistBookAsync(bookDto.Author, bookDto.Title), Times.Once);
        repoMock
            .Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}