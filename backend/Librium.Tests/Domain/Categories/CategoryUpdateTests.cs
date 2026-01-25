using FluentAssertions;
using Librium.Domain.Categories;

namespace Librium.Tests.Domain.Categories;

public class CategoryUpdateTests
{
    private static Category CreateCategory()
        => Category.Create("Science").Value;

    [Fact]
    public void Update_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var category = CreateCategory();

        //act
        var result = category!.Update("Education");

        //assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("Education");
    }

    [Fact]
    public void Update_ShouldFail_WhenInputIsInvalid()
    {
        //arrange
        var category = CreateCategory();

        //act
        var result = category!.Update("");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Update_ShouldFail_WhenNameTooLong()
    {
        //arrange
        var category = CreateCategory();

        //act
        var result = category!.Update(new string('A', 101));

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
