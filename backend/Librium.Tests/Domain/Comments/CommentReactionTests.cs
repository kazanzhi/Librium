using FluentAssertions;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Enums;

namespace Librium.Tests.Domain.Comments;

public class CommentReactionTests
{
    private static Comment CreateComment()
        => Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "test comment", DateTime.UtcNow).Value;

    [Fact]
    public void React_ShouldAddLike_WhenUserHasNoReaction()
    {
        //arrange
        var comment = CreateComment();
        var userId = Guid.NewGuid();

        //act
        var reactResult = comment.React(userId, ReactionType.Like);

        //assert
        reactResult.IsSuccess.Should().BeTrue();
        comment.TotalLikes.Should().Be(1);
        comment.TotalDislikes.Should().Be(0);
        comment.Reactions.Count().Should().Be(1);
    }

    [Fact]
    public void React_ShouldFail_WhenUserIdIsEmpty()
    {
        //arrange
        var comment = CreateComment();
        var userId = Guid.Empty;

        //act
        var reactResult = comment.React(userId, ReactionType.Like);

        //assert
        reactResult.IsSuccess.Should().BeFalse();
        reactResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void React_ShouldRemoveReaction_WhenSameReactionIsAppliedTwice()
    {
        //arrange
        var comment = CreateComment();
        var userId = Guid.NewGuid();
        comment.React(userId, ReactionType.Like);

        //act
        var reactResult = comment.React(userId, ReactionType.Like);

        //assert
        reactResult.IsSuccess.Should().BeTrue();
        comment.TotalDislikes.Should().Be(0);
        comment.TotalLikes.Should().Be(0);
        comment.Reactions.Count().Should().Be(0);
    }

    [Fact]
    public void React_ShouldSwitchReaction_WhenDifferentReactionIsApplied()
    {
        //arrange
        var comment = CreateComment();
        var userId = Guid.NewGuid();
        comment.React(userId, ReactionType.Like);

        //act
        var reactResult = comment.React(userId, ReactionType.Dislike);

        //assert
        reactResult.IsSuccess.Should().BeTrue();
        comment.TotalDislikes.Should().Be(1);
        comment.TotalLikes.Should().Be(0);
        comment.Reactions.Count.Should().Be(1);
    }
}