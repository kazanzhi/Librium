using FluentAssertions;
using Librium.Application.Books.Commands.UpdateBook;
using Librium.Application.Books.DTOs;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Commands;

public class UpdateBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenBookExists()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var dto = new BookDto
        {
            Title = "NewTitle",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2001
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new UpdateBookCommandHandler(repoMock.Object);
        var command = new UpdateBookCommand(book.Id, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be(dto.Title);
        book.Author.Should().Be(dto.Author);
        book.Content.Should().Be(dto.Content);
        book.PublishedYear.Should().Be(dto.PublishedYear);

        repoMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookDoesNotExist()
    {
        //arrange
        var bookId = Guid.NewGuid();
        var dto = new BookDto
        {
            Title = "NewTitle",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2001
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        var handler = new UpdateBookCommandHandler(repoMock.Object);
        var command = new UpdateBookCommand(bookId, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDomainValidationFails()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var dto = new BookDto
        {
            Title = "",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2001
        };

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new UpdateBookCommandHandler(repoMock.Object);
        var command = new UpdateBookCommand(book.Id, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}