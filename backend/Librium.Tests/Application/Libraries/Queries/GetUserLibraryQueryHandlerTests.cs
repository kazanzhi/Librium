using FluentAssertions;
using Librium.Application.Books.DTOs;
using Librium.Application.Categories.DTOs;
using Librium.Application.Libraries.Queries.GetUserLibrary;
using Librium.Application.Libraries.Repositories;
using Librium.Domain.Books;
using Moq;

namespace Librium.Tests.Application.Libraries.Queries;

public class GetUserLibraryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedDto_WhenUserBooksExist()
    {
        //arrange
        var userId = Guid.NewGuid();
        var books = new List<Book>
        {
            Book.Create("Title", "Author", "Content", 2000).Value!,
            Book.Create("Title2", "Author2", "Content1", 2001).Value!
        };

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.GetBooksByUserIdAsync(userId)).ReturnsAsync(books);

        var handler = new GetUserLibraryQueryHandler(userBookRepo.Object);
        var query = new GetUserLibraryQuery(userId);

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
                }
            }
        );

        userBookRepo
            .Verify(r => r.GetBooksByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBooksFoundForUser()
    {
        //arrange
        var userId = Guid.NewGuid();

        var userBookRepo = new Mock<IUserBookRepository>();
        userBookRepo
            .Setup(r => r.GetBooksByUserIdAsync(userId)).ReturnsAsync(new List<Book>());

        var handler = new GetUserLibraryQueryHandler(userBookRepo.Object);
        var query = new GetUserLibraryQuery(userId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().BeEmpty();

        userBookRepo
            .Verify(r => r.GetBooksByUserIdAsync(userId), Times.Once);
    }
}