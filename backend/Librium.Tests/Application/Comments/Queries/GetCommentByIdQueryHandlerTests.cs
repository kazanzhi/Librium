using FluentAssertions;
using Librium.Application.Comments.DTOs;
using Librium.Application.Comments.Queries.GetCommentById;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Queries;
public class GetCommentByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedComment_WhenCommentExists()
    {
        //arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var comment = Comment.Create(userId, bookId, "Content", DateTime.UtcNow).Value!;

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(comment.Id)).ReturnsAsync(comment);

        var handler = new GetCommentByIdQueryHandler(repoMock.Object);
        var query = new GetCommentByIdQuery(comment.Id);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(
            new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                TotalLikes = comment.TotalLikes,
                TotalDislikes = comment.TotalDislikes,
                IsEdited = comment.IsEdited
            }    
        );

        repoMock
            .Verify(r => r.GetByIdAsync(comment.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommentDoesNotExist()
    {
        //arrange
        var commentId = Guid.NewGuid();

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment?)null);

        var handler = new GetCommentByIdQueryHandler(repoMock.Object);
        var query = new GetCommentByIdQuery(commentId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();
    }
}