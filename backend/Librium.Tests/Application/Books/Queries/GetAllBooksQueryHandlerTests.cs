using FluentAssertions;
using Librium.Application.Books.DTOs;
using Librium.Application.Books.Queries.GetAllBooks;
using Librium.Application.Categories.DTOs;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Moq;
using System.Net;

namespace Librium.Tests.Application.Books.Queries;

public class GetAllBooksQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedBooks_WhenBooksExist()
    {
        //arrange
        var books = new List<Book>
        {
            Book.Create("Title", "Author", "Content", 2000).Value!,
            Book.Create("Title1", "Author2", "Content2", 2000).Value!
        };


        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetAllBooks(null)).ReturnsAsync(books);


        var handler = new GetAllBooksQueryHandler(repoMock.Object);
        var query = new GetAllBooksQuery(null);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new List<BookResponseDto>
        {
            new BookResponseDto { 
                Id = books[0].Id,
                Title = books[0].Title,
                Author = books[0].Author,
                Content = books[0].Content,
                PublishedYear = books[0].PublishedYear,
                Categories = new List<CategoryResponseDto>()
            },
            new BookResponseDto {
                Id = books[1].Id,
                Title = books[1].Title,
                Author = books[1].Author,
                Content = books[1].Content,
                PublishedYear = books[1].PublishedYear,
                Categories = new List<CategoryResponseDto>()
            },
        });

        repoMock.Verify(r => r.GetAllBooks(null), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBooksExist()
    {
        //arrange
        var repoMock = new Mock<IBookRepository>();
        repoMock
            .Setup(r => r.GetAllBooks(It.IsAny<string>()))
            .ReturnsAsync(new List<Book>());

        var handler = new GetAllBooksQueryHandler(repoMock.Object);
        var query = new GetAllBooksQuery(null);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();

        repoMock.Verify(r => r.GetAllBooks(null), Times.Once);
    }
}