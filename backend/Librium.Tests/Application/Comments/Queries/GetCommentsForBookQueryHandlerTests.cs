using FluentAssertions;
using Librium.Application.Comments.DTOs;
using Librium.Application.Comments.Queries.GetCommentsForBook;
using Librium.Domain.Comments;
using Librium.Domain.Comments.Repositories;
using Moq;

namespace Librium.Tests.Application.Comments.Queries;

public class GetCommentsForBookQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedComments_WhenCommentsExist()
    {
        //arrange
        var bookId = Guid.NewGuid();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var comments = new List<Comment>
        {
            Comment.Create(userId1, bookId, "Content", DateTime.UtcNow).Value!,
            Comment.Create(userId2, bookId, "Other Content", DateTime.UtcNow).Value!
        };

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByBookIdAsync(bookId)).ReturnsAsync(comments);

        var handler = new GetCommentsForBookQueryHandler(repoMock.Object);
        var query = new GetCommentsForBookQuery(bookId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            new List<CommentResponseDto>{
                new CommentResponseDto
                 {
                     Id = comments[0].Id,
                     Content = comments[0].Content,
                     CreatedAt = comments[0].CreatedAt,
                     TotalLikes = comments[0].TotalLikes,
                     TotalDislikes = comments[0].TotalDislikes,
                     IsEdited = comments[0].IsEdited
                 },
                new CommentResponseDto
                {
                    Id = comments[1].Id,
                    Content = comments[1].Content,
                    CreatedAt = comments[1].CreatedAt,
                    TotalLikes = comments[1].TotalLikes,
                    TotalDislikes = comments[1].TotalDislikes,
                    IsEdited = comments[1].IsEdited
                }
            }
        );

        repoMock
            .Verify(r => r.GetByBookIdAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoComments()
    {
        //arrange
        var bookId = Guid.NewGuid();

        var repoMock = new Mock<ICommentRepository>();
        repoMock
            .Setup(r => r.GetByBookIdAsync(bookId)).ReturnsAsync(new List<Comment>());

        var handler = new GetCommentsForBookQueryHandler(repoMock.Object);
        var query = new GetCommentsForBookQuery(bookId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();

        repoMock
            .Verify(r => r.GetByBookIdAsync(bookId), Times.Once);
    }
}