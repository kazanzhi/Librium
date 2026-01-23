using FluentAssertions;
using Librium.Domain.Categories;

namespace Librium.Tests.Domain;

public class BookCategoryTests
{
    //create
    [Fact]
    public void Create_ShouldReturnSuccess_WhenNameIsValid()
    {
        //arrange

        //act
        var result = Category.Create("Science");

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("Science");
        result.Value.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_ShouldReturnTrimName_WhenNameHasWhiteSpaces()
    {
        //arrange

        //act
        var result = Category.Create("    Science   ");

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("Science");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsEmpty()
    {
        //arrange

        //act
        var result = Category.Create("");

        //asserts
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name is required.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameTooLong()
    {
        //arrange 

        //act
        var result = Category.Create(new string('A', 101));

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name cannot exceed 100 characters.");
    }

    //update
    [Fact]
    public void Update_ShouldReturnSuccess_WhenNameIsValid()
    {
        //arrange
        var category = Category.Create("Science").Value;

        //act
        var result = category!.Update("Education");

        //assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("Education");
    }

    [Fact]
    public void Update_ShouldReturnTrimName_WhenNameHasWhiteSpaces()
    {
        //arrange
        var category = Category.Create("Science").Value;

        //act
        var result = category!.Update("   Education   ");

        //assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("Education");
    }

    [Fact]
    public void Update_ShouldNotChangeId_WhenUpdatingCategory()
    {
        //arrange
        var category = Category.Create("Science").Value;
        var categoryId = category!.Id;

        //act
        var result = category.Update("Education");

        //assert
        category.Id.Should().Be(categoryId);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenNameIsEmpty()
    {
        //arrange
        var category = Category.Create("Science").Value;

        //act
        var result = category!.Update("");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name is required.");
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenNameTooLong()
    {
        //arrange
        var category = Category.Create("Science").Value;

        //act
        var result = category!.Update(new string('A', 101));

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name cannot exceed 100 characters.");
    }
}
