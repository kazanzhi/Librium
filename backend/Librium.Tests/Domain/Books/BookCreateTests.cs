using FluentAssertions;
using Librium.Domain.Books;

namespace Librium.Tests.Domain.Books;

public class BookCreateTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenInputIsValid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("TestTitle");
        result.Value.Author.Should().Be("TestAuthor");
        result.Value.Content.Should().Be("TestContent");
        result.Value.PublishedYear.Should().Be(2000);
    }

    [Fact]
    public void Create_ShouldFail_WhenTitleIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create(null, "TestAuthor", "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenAuthorIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", null, "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenContentIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", null, 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenPublishedYearIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", "TestContent", -10);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
