using FluentAssertions;
using Librium.Application.Books.DTOs;
using Librium.Application.Books.Queries.GetBookById;
using Librium.Application.Categories.DTOs;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Queries;

public class GetBookByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedBook_WhenBookExists()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new GetBookByIdQueryHandler(repoMock.Object);
        var query = new GetBookByIdQuery(book.Id);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Content = book.Content,
            PublishedYear = book.PublishedYear,
            Categories = new List<CategoryResponseDto>()
        });

        repoMock.Verify(r => r.GetBookById(book.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookDoesNotExists()
    {
        //arrange
        var bookId = Guid.NewGuid();

        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        var handler = new GetBookByIdQueryHandler(repoMock.Object);
        var query = new GetBookByIdQuery(bookId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetBookById(bookId), Times.Once);
    }
}