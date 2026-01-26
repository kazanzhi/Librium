using FluentAssertions;
using Librium.Domain.Comments;

namespace Librium.Tests.Domain.Comments;

public class CommentCreateTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenInputIsValid()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        //act
        var result = Comment.Create(userId, bookId, "test comment", createdAt);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.BookId.Should().Be(bookId);
        result.Value.UserId.Should().Be(userId);
        result.Value.IsEdited.Should().BeFalse();
        result.Value.TotalLikes.Should().Be(0);
        result.Value.TotalDislikes.Should().Be(0);
        result.Value.Content.Should().Be("test comment");
        result.Value.CreatedAt.Should().Be(createdAt);
    }

    [Fact]
    public void Create_ShouldFail_WhenContentIsInvalid()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        //act
        var result = Comment.Create(userId, bookId, "", createdAt);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenUserIdIsEmpty()
    {
        //arrange
        var userId = Guid.Empty;
        var bookId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        //act
        var result = Comment.Create(userId, bookId, "test comment", createdAt);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Create_ShouldFail_WhenBookIdIsEmpty()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.Empty;
        var createdAt = DateTime.UtcNow;

        //act
        var result = Comment.Create(userId, bookId, "test comment", createdAt);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
