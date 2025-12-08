using FluentAssertions;
using Librium.Domain.Books.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Librium.Tests.Domain;

public class BookTests
{
    private static Guid ValidCategoryId() => BookCategory.Create("TestsCategory").Value!.Id;

    //create
    [Fact]
    public void Create_ShouldReturnSuccess_WhenInputsValid()
    {
        //arrange
        var categoryId = ValidCategoryId();

        //act
        var result = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("TestTitle");
        result.Value.Author.Should().Be("TestAuthor");
        result.Value.CategoryId.Should().Be(categoryId);
        result.Value.Content.Should().Be("TestContent");
        result.Value.PublishedYear.Should().Be(2000);
    }

    [Fact]
    public void Create_ShouldReturnTrimTitle_WhenTitleHasExtraSpaces()
    {
        //arrange

        //act
        var result = Book.Create("   TestTitle   ", "TestAuthor", ValidCategoryId(), "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("TestTitle");
    }

    [Fact]
    public void Create_ShouldReturnTrimAuthor_WhenAuthorHasExtraSpaces()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "   TestAuthor   ", ValidCategoryId(), "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Author.Should().Be("TestAuthor");
    }

    [Fact]
    public void Create_ShouldReturnTrimContent_WhenContentHasExtraSpaces()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", ValidCategoryId(), "  TestContent  ", 2000);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Content.Should().Be("TestContent");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenTitleIsNull()
    {
        //arrange

        //act
        var result = Book.Create(null, "TestAuthor", ValidCategoryId(), "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenAuthorIsNull()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", null, ValidCategoryId(), "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Author is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenCategoryIdIsEmpty()
    {
        //arrange
        

        //act
        var result = Book.Create("TestTitle", "TestAuthor", Guid.Empty, "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenContentIsNull()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", ValidCategoryId(), null, 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Content is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenPublishedYearIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", ValidCategoryId(), "TestContent", -10);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid published year.");
    }

    //update
    [Fact]
    public void Update_ShouldReturnSuccess_WhenInputsValid()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be("UpdatedTitle");
        book.Author.Should().Be("UpdatedAuthor");
        book.Content.Should().Be("UpdatedContent");
        book.PublishedYear.Should().Be(1980);
        book.BookCategory.Should().Be(newCategory);
    }

    [Fact]
    public void Update_ShouldReturnTrimTitle_WhenTitleHasExtraSpaces()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("  UpdatedTitle   ", "UpdatedAuthor", "UpdatedContent", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be("UpdatedTitle");
    }

    [Fact]
    public void Update_ShouldReturnTrimAuthor_WhenAuthorHasExtraSpaces()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "   UpdatedAuthor   ", "UpdatedContent", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Author.Should().Be("UpdatedAuthor");
    }

    [Fact]
    public void Update_ShouldReturnTrimContent_WhenContentHasExtraSpaces()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "   UpdatedContent   ", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Content.Should().Be("UpdatedContent");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenTitleIsNull()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update(null, "UpdatedAuthor", "UpdatedContent", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenAuthorIsNull()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", null, "UpdatedContent", 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Author is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenContentIsNull()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", null, 1980, newCategory);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Content is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenCategoryIsNull()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", 1980, null);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenPublishedYearIsInvalid()
    {
        //arrange
        var categoryId = ValidCategoryId();
        var book = Book.Create("TestTitle", "TestAuthor", categoryId, "TestContent", 2000).Value!;
        var newCategory = BookCategory.Create("Science").Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", -10, newCategory);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid published year.");
    }
}