using FluentAssertions;
using Librium.Application.Categories.Commands.UpdateCategory;
using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Categories.Commands;

public class UpdateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCategoryExists()
    {
        //arrange
        var category = Category.Create("Test").Value!;
        var dto = new CategoryDto { Name = "Test" };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        var handler = new UpdateCategoryCommandHandler(repoMock.Object);
        var command = new UpdateCategoryCommand(category.Id, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoMock.Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryNotExists()
    {
        //arrange
        var categoryId = Guid.NewGuid();
        var dto = new CategoryDto { Name = "Test" };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        var handler = new UpdateCategoryCommandHandler(repoMock.Object);
        var command = new UpdateCategoryCommand(categoryId, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryUpdateIsInvalid()
    {
        //arrange
        var category = Category.Create("Test").Value!;
        var dto = new CategoryDto { Name = new string('a', 51) };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        var handler = new UpdateCategoryCommandHandler(repoMock.Object);
        var command = new UpdateCategoryCommand(category.Id, dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}