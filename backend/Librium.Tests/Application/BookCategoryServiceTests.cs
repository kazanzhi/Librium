using FluentAssertions;
using Librium.Application.DTOs.Categories;
using Librium.Application.Services.Categories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application;

public class BookCategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repo;
    private readonly CategoryService _service;

    public BookCategoryServiceTests()
    {
        _repo = new Mock<ICategoryRepository>();
        _service = new CategoryService(_repo.Object);
    }

    //create
    [Fact]
    public async Task CreateBookCategoryAsync_ShouldCreateCategory_WhenNotExists()
    {
        //arrange
        var dto = new CategoryDto { Name = "Science" };
        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _repo.Verify(r => r.AddBookCategory(It.IsAny<Category>()), Times.Once());
        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldTrimName()
    {
        //arrange
        var dto = new CategoryDto { Name = "   Science   " };
        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeTrue();

        _repo.Verify(r => r.AddBookCategory(It.Is<Category>(c => c.Name == "Science")), Times.Once());
        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenCategoryExists()
    {
        //arrange
        var dto = new CategoryDto { Name = "Science" };
        var existingCategoryResult = Category.Create("Science");
        existingCategoryResult.IsSuccess.Should().BeTrue();
        var existingCategory = existingCategoryResult.Value!;

        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingCategory);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("This category already exists.");

        _repo.Verify(r => r.AddBookCategory(It.IsAny<Category>()), Times.Never());
        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenCategoryExistsWithTrim()
    {
        //arrange
        var dto = new CategoryDto { Name = "   Science   " };
        var existingCategoryResult = Category.Create("Science");
        existingCategoryResult.IsSuccess.Should().BeTrue();
        var existingCategory = existingCategoryResult.Value!;

        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(existingCategory);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("This category already exists.");
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenNameIsEmpty()
    {
        //arrange
        var dto = new CategoryDto { Name = "" };
        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name is required.");
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenNameIsTooLong()
    {
        //arrange
        var dto = new CategoryDto { Name = new string('A', 101) };
        _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name cannot exceed 100 characters.");
    }

    //Delete
    [Fact]
    public async Task DeleteBookCategoryAsync_ShouldReturnSuccess_WhenCategoryDeleted()
    {
        //arrange 
        var created = Category.Create("Science");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.DeleteBookCategoryAsync(category.Id);

        //assert
        result.IsSuccess.Should().BeTrue();

        _repo.Verify(r => r.Delete(It.Is<Category>(c => c.Name == "Science")), Times.Once());
        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task DeleteBookCategoryAsync_ShouldReturnFailure_WhenNoCategory()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.DeleteBookCategoryAsync(Guid.NewGuid());

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category not found.");

        _repo.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never());
        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    //getAll
    [Fact]
    public async Task GetAllBookCategoriesAsync_ShouldReturnMappedList()
    {
        //arrange
        var a = Category.Create("A");
        var b = Category.Create("B");

        a.IsSuccess.Should().BeTrue();
        b.IsSuccess.Should().BeTrue();

        _repo.Setup(r => r.GetAllBookCategoriesAsync()).ReturnsAsync(new List<Category> { a.Value!, b.Value! });

        //act
        var result = await _service.GetAllBookCategoriesAsync();

        //assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be(expected: "A");
        result[1].Name.Should().Be("B");
        result.Should().BeOfType<List<CategoryResponseDto>>();
    }

    [Fact]
    public async Task GetAllBookCategoriesAsync_ShouldReturnEmptyList_WhenNoCategories()
    {
        //arrange
        _repo.Setup(r => r.GetAllBookCategoriesAsync()).ReturnsAsync(new List<Category>());

        //act
        var result = await _service.GetAllBookCategoriesAsync();

        //assert
        result.Should().BeEmpty();

        _repo.Verify(r => r.GetAllBookCategoriesAsync(), Times.Once());
    }

    //getById
    [Fact]
    public async Task GetBookCategoryById_ShouldReturnMappedDto()
    {
        //arrange 
        var created = Category.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.GetBookCategoryById(category.Id);

        //assert
        result.Should().BeOfType<CategoryResponseDto>();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be("A");
    }

    [Fact]
    public async Task GetBookCategoryById_ShouldThrow_WhenCategoryNotFound()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        //act
        var result = async () => await _service.GetBookCategoryById(Guid.NewGuid());

        //assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    //update
    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnSucces_WhenCategoryUpdated()
    {
        //arrange
        var created = Category.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new CategoryDto { Name = "B" });

        //assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("B");

        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenCategoryNotFound()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        //act
        var result = await _service.UpdateBookCategoryAsync(Guid.NewGuid(), new CategoryDto { Name = "B" });

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category not found.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenNameIsEmpty()
    {
        //arrange
        var created = Category.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new CategoryDto { Name = ""});

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name is required.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenNameTooLong()
    {
        //arrange
        var created = Category.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryByIdAsync(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new CategoryDto { Name = new string('A', 101) });

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name cannot exceed 100 characters.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }
}