using FluentAssertions;
using Librium.Domain.Books;
using Librium.Domain.Categories;

namespace Librium.Tests.Domain.Books;

public class BookCategoryTests
{
    private static Book CreateBook()
        => Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value;

    private static Category CreateCategory()
        => Category.Create("TestCategory").Value;

    [Fact]
    public void AddCategory_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var book = CreateBook();
        var category = CreateCategory();

        //act
        var result = book.AddCategory(category);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Categories.Should().ContainSingle();
        book.Categories.First().Id.Should().Be(category.Id);
    }

    [Fact]
    public void AddCategory_ShouldFail_WhenCategoryIsNull()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.AddCategory(null);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void AddCategory_ShouldFail_WhenCategoryAlreadyAssigned()
    {
        //arrange
        var book = CreateBook();
        var category = CreateCategory();

        book.AddCategory(category);

        //act
        var result = book.AddCategory(category);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        book.Categories.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveCategory_ShouldSucceed_WhenCategoryIsValid()
    {
        //arrange
        var book = CreateBook();
        var category = CreateCategory();

        book.AddCategory(category);

        //act
        var result = book.RemoveCategory(category);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Categories.Should().BeEmpty();
    }

    [Fact]
    public void RemoveCategory_ShouldFail_WhenCategoryIsNull()
    {
        //arrange
        var book = CreateBook();

        //act
        var result = book.RemoveCategory(null);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RemoveCategory_ShouldFail_WhenCategoryIsNotAssigned()
    {
        //arrange
        var book = CreateBook();
        var category = CreateCategory();

        //act
        var result = book.RemoveCategory(category);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
