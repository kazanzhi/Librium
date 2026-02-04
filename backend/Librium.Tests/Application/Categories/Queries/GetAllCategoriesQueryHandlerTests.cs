using FluentAssertions;
using Librium.Application.Categories.DTOs;
using Librium.Application.Categories.Queries.GetAllCategories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Categories.Queries;

public class GetAllCategoriesQueryHandlerTests
{

    [Fact]
    public async Task Handle_ShouldReturnMappedCategories_WhenCategoriesExist()
    {
        //arrange
        var categories = new List<Category>
        {
            Category.Create("Science").Value!,
            Category.Create("Education").Value!,
        };

        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);

        var handler = new GetAllCategoriesQueryHandler(repoMock.Object);
        var query = new GetAllCategoriesQuery();

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new List<CategoryResponseDto>
        {
            new CategoryResponseDto { Id = categories[0].Id, Name = "Science"},
            new CategoryResponseDto { Id = categories[1].Id, Name = "Education"}
        });

        repoMock.Verify(r => r.GetAllCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        //arrange
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>());

        var handler = new GetAllCategoriesQueryHandler(repoMock.Object);
        var query = new GetAllCategoriesQuery();

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().BeEmpty();

        repoMock.Verify(r => r.GetAllCategoriesAsync(), Times.Once);
    }
}