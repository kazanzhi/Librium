using FluentAssertions;
using Librium.Domain.Categories;

namespace Librium.Tests.Domain.Categories;

public class CategoryCreateTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenNameIsValid()
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
    public void Create_ShouldFail_WhenNameIsInvalid()
    {
        //arrange

        //act
        var result = Category.Create("");

        //asserts
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenNameTooLong()
    {
        //arrange 

        //act
        var result = Category.Create(new string('A', 101));

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
