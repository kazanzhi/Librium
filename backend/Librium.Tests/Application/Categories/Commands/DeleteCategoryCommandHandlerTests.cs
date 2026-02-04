using FluentAssertions;
using Librium.Application.Categories.Commands.DeleteCategory;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Categories.Commands;

public class DeleteCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCategoryExists()
    {
        //arrange
        var category = Category.Create("Test").Value!;

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        var handler = new DeleteCategoryCommandHandler(repoMock.Object);
        var command = new DeleteCategoryCommand(category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoMock.Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        repoMock.Verify(r => r.Delete(category), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryNotExists()
    {
        //arrange
        var categoryId = Guid.NewGuid();

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        var handler = new DeleteCategoryCommandHandler(repoMock.Object);
        var command = new DeleteCategoryCommand(categoryId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
        repoMock.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}