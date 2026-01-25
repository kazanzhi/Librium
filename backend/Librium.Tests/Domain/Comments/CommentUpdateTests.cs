using FluentAssertions;
using Librium.Domain.Comments;

namespace Librium.Tests.Domain.Comments;

public class CommentUpdateTests
{
    private static Comment CreateComment()
        => Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "old test comment", DateTime.UtcNow).Value;
    [Fact]
    public void Update_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var comment = CreateComment();

        //act
        var updateResult = comment.Update("new test comment");

        //assert
        updateResult.IsSuccess.Should().BeTrue();
        comment.Content.Should().Be("new test comment");
        comment.IsEdited.Should().BeTrue();
    }

    [Fact]
    public void Update_ShouldFail_WhenInputIsInvalid()
    {
        //arrange
        var comment = CreateComment();

        //act
        var updatedResult = comment.Update("ne");

        //assert
        updatedResult.IsSuccess.Should().BeFalse();
        updatedResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}