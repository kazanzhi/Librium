using FluentAssertions;
using Librium.Domain.Books.Models;

namespace Librium.Tests.Domain;

public class BookTests
{
    //create
    [Fact]
    public void Create_ShouldReturnSuccess_WhenInputsValid()
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
    public void Create_ShouldReturnTrimTitle_WhenTitleHasExtraSpaces()
    {
        //arrange

        //act
        var result = Book.Create("   TestTitle   ", "TestAuthor", "TestContent", 2000);

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
        var result = Book.Create("TestTitle", "   TestAuthor   ","TestContent", 2000);

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
        var result = Book.Create("TestTitle", "TestAuthor", "  TestContent  ", 2000);

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
        var result = Book.Create(null, "TestAuthor","TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenAuthorIsNull()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", null, "TestContent", 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Author is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenContentIsNull()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", null, 2000);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Content is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenPublishedYearIsInvalid()
    {
        //arrange

        //act
        var result = Book.Create("TestTitle", "TestAuthor", "TestContent", -10);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid published year.");
    }

    //update
    [Fact]
    public void Update_ShouldReturnSuccess_WhenInputsValid()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

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
    public void Update_ShouldReturnTrimTitle_WhenTitleHasExtraSpaces()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("  UpdatedTitle   ", "UpdatedAuthor", "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be("UpdatedTitle");
    }

    [Fact]
    public void Update_ShouldReturnTrimAuthor_WhenAuthorHasExtraSpaces()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("UpdatedTitle", "   UpdatedAuthor   ", "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Author.Should().Be("UpdatedAuthor");
    }

    [Fact]
    public void Update_ShouldReturnTrimContent_WhenContentHasExtraSpaces()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "   UpdatedContent   ", 1980);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Content.Should().Be("UpdatedContent");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenTitleIsNull()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update(null, "UpdatedAuthor", "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenAuthorIsNull()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("UpdatedTitle", null, "UpdatedContent", 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Author is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenContentIsNull()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", null, 1980);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Content is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenPublishedYearIsInvalid()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;

        //act
        var result = book.Update("UpdatedTitle", "UpdatedAuthor", "UpdatedContent", -10);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid published year.");
    }

    //addCaregory
    [Fact]
    public void AddCategory_ShouldAddCategory_WhenCategoryIsValid()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;
        var categoryResult = BookCategory.Create("TestCategory");
        categoryResult.IsSuccess.Should().BeTrue();
        var category = categoryResult.Value;

        //act
        var result = book.AddCategory(category);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.BookCategories.Should().ContainSingle();
        book.BookCategories.First().Id.Should().Be(category.Id);
    }

    [Fact]
    public void AddCategory_ShouldReturnFailure_WhenNoCategory()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;

        //act
        var result = book.AddCategory(null);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category is required.");
    }

    [Fact]
    public void AddCategory_ShouldReturnFailure_WhenCategoryAlreadyAssigned()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;
        var categoryResult = BookCategory.Create("TestCategory");
        categoryResult.IsSuccess.Should().BeTrue();
        var category = categoryResult.Value;
        book.AddCategory(category);

        //act
        var result = book.AddCategory(category);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category already assigned.");
        book.BookCategories.Should().HaveCount(1);
    }

    //removeCategory
    [Fact]
    public void RemoveCategory_ShouldReturnSuccess_WhenCategoryIsRemoved()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;
        var categoryResult = BookCategory.Create("TestCategory");
        categoryResult.IsSuccess.Should().BeTrue();
        var category = categoryResult.Value;
        book.AddCategory(category);

        //act
        var result = book.RemoveCategory(category);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.BookCategories.Should().BeEmpty();
    }

    [Fact]
    public void RemoveCategory_ShouldReturnFailure_WhenCategoryIsNull()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;

        //act
        var result = book.RemoveCategory(null);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category is required.");
    }

    [Fact]
    public void RemoveCategory_ShouldReturnFailure_WhenCategoryNotFound()
    {
        //arrange
        var bookResult = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000);
        bookResult.IsSuccess.Should().BeTrue();
        var book = bookResult.Value;
        var categoryResult = BookCategory.Create("TestCategory");
        categoryResult.IsSuccess.Should().BeTrue();
        var category = categoryResult.Value;

        //act
        var result = book.RemoveCategory(category);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category not found.");
    }
}