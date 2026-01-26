using FluentAssertions;
using Librium.Domain.Books;

namespace Librium.Tests.Domain.Books;

public class BookUpdateTests
{
    private static Book CreateBook() 
        => Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value;

    [Fact]
    public void Update_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be("UpdatedTitle");
        book.Author.Should().Be("UpdatedAuthor");
        book.Content.Should().Be("UpdatedContent");
        book.PublishedYear.Should().Be(1980);
    }

    [Fact]
    public void Update_ShouldFail_WhenTitleIsInvalid()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.Update(null, "UpdatedAuthor", "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Update_ShouldFail_WhenAuthorIsInvalid()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.Update("UpdatedTitle", null, "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Update_ShouldFail_WhenContentIsInvalid()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", null, 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Update_ShouldFail_WhenPublishedYearIsInvalid()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", -10);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
