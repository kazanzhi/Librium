using FluentAssertions;
using Librium.Application.Categories.DTOs;
using Librium.Application.Categories.Queries.GetCategoryById;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedCategory_WhenCategoryExists()
    {
        //arrange
        var category = Category.Create("Science").Value!;
        var categoryId = Guid.NewGuid();
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

        var handler = new GetCategoryByIdQueryHandler(repoMock.Object);
        var query = new GetCategoryByIdQuery(categoryId);

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new CategoryResponseDto { Id = category.Id, Name = "Science" });

        repoMock.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryNotExists()
    {
        //arrange
        var respoMock = new Mock<ICategoryRepository>();
        respoMock.Setup(r => r.GetCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        var hanler = new GetCategoryByIdQueryHandler(respoMock.Object);
        var query = new GetCategoryByIdQuery(It.IsAny<Guid>());

        //act
        var result = await hanler.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        respoMock.Verify(r => r.GetCategoryByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}