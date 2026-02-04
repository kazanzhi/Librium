using FluentAssertions;
using Librium.Application.Categories.Commands.CreateCategory;
using Librium.Application.Categories.DTOs;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCategoryIsNew()
    {
        //arrange
        var name = "testCategory";
        var dto = new CategoryDto { Name = name };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetByNameAsync(name))
            .ReturnsAsync((Category?)null);

        var handler = new CreateCategoryCommandHandler(repoMock.Object);
        var command = new CreateCategoryCommand(dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        repoMock.Verify(r => r.GetByNameAsync(name), Times.Once);
        repoMock.Verify(r => r.Add(It.Is<Category>(c => c.Name == name)), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryAlreadyExists()
    {
        //arrange
        var name = "testCategory";
        var dto = new CategoryDto { Name = name };
        var category = Category.Create("testCategory").Value!;

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetByNameAsync(name))
            .ReturnsAsync(category);

        var handler = new CreateCategoryCommandHandler(repoMock.Object);
        var command = new CreateCategoryCommand(dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetByNameAsync(name), Times.Once);
        repoMock.Verify(r => r.Add(It.IsAny<Category>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryInputIsInvalid()
    {
        //arrange
        var name = new string('a', 51);
        var dto = new CategoryDto { Name = name };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(r => r.GetByNameAsync(name))
            .ReturnsAsync((Category?)null);

        var handler = new CreateCategoryCommandHandler(repoMock.Object);
        var command = new CreateCategoryCommand(dto);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoMock.Verify(r => r.GetByNameAsync(name), Times.Once);
        repoMock.Verify(r => r.Add(It.Is<Category>(c => c.Name == name)), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}