using FluentAssertions;
using Librium.Application.Services;
using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Books.Repositories;
using Librium.Domain.DTOs.BookCategories;
using Moq;

namespace Librium.Tests.Application;

public class BookCategoryServiceTests
{
    private readonly Mock<IBookCategoryRepository> _repo;
    private readonly BookCategoryService _service;

    public BookCategoryServiceTests()
    {
        _repo = new Mock<IBookCategoryRepository>();
        _service = new BookCategoryService(_repo.Object);
    }

    //create
    [Fact]
    public async Task CreateBookCategoryAsync_ShouldCreateCategory_WhenNotExists()
    {
        //arrange
        var dto = new BookCategoryDto { Name = "Science" };
        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _repo.Verify(r => r.AddBookCategory(It.IsAny<BookCategory>()), Times.Once());
        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldTrimName()
    {
        //arrange
        var dto = new BookCategoryDto { Name = "   Science   " };
        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeTrue();

        _repo.Verify(r => r.AddBookCategory(It.Is<BookCategory>(c => c.Name == "Science")), Times.Once());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenCategoryExists()
    {
        //arrange
        var dto = new BookCategoryDto { Name = "Science" };
        var existingCategoryResult = BookCategory.Create("Science");
        existingCategoryResult.IsSuccess.Should().BeTrue();
        var existingCategory = existingCategoryResult.Value!;

        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { existingCategory });

        //act
        var result = await _service.CreateBookCategoryAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("This category already exists.");

        _repo.Verify(r => r.AddBookCategory(It.IsAny<BookCategory>()), Times.Never());
        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task CreateBookCategoryAsync_ShouldReturnFailure_WhenCategoryExistsWithTrim()
    {
        //arrange
        var dto = new BookCategoryDto { Name = "   Science   " };
        var existingCategoryResult = BookCategory.Create("Science");
        existingCategoryResult.IsSuccess.Should().BeTrue();
        var existingCategory = existingCategoryResult.Value!;

        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { existingCategory });

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
        var dto = new BookCategoryDto { Name = "" };
        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

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
        var dto = new BookCategoryDto { Name = new string('A', 101) };
        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

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
        var created = BookCategory.Create("Science");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryById(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.DeleteBookCategoryAsync(category.Id);

        //assert
        result.IsSuccess.Should().BeTrue();

        _repo.Verify(r => r.Delete(It.Is<BookCategory>(c => c.Name == "Science")), Times.Once());
        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task DeleteBookCategoryAsync_ShouldReturnFailure_WhenNoCategory()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryById(It.IsAny<Guid>())).ReturnsAsync((BookCategory?)null);

        //act
        var result = await _service.DeleteBookCategoryAsync(Guid.NewGuid());

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category not found.");

        _repo.Verify(r => r.Delete(It.IsAny<BookCategory>()), Times.Never());
        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    //getAll
    [Fact]
    public async Task GetAllBookCategoriesAsync_ShouldReturnMappedList()
    {
        //arrange
        var a = BookCategory.Create("A");
        var b = BookCategory.Create("B");

        a.IsSuccess.Should().BeTrue();
        b.IsSuccess.Should().BeTrue();

        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { a.Value!, b.Value! });

        //act
        var result = await _service.GetAllBookCategoriesAsync();

        //assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be(expected: "A");
        result[1].Name.Should().Be("B");
        result.Should().BeOfType<List<BookCategoryResponseDto>>();
    }

    [Fact]
    public async Task GetAllBookCategoriesAsync_ShouldReturnEmptyList_WhenNoCategories()
    {
        //arrange
        _repo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

        //act
        var result = await _service.GetAllBookCategoriesAsync();

        //assert
        result.Should().BeEmpty();

        _repo.Verify(r => r.GetAllBookCategories(), Times.Once());
    }

    //getById
    [Fact]
    public async Task GetBookCategoryById_ShouldReturnMappedDto()
    {
        //arrange 
        var created = BookCategory.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryById(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.GetBookCategoryById(category.Id);

        //assert
        result.Should().BeOfType<BookCategoryResponseDto>();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be("A");
    }

    [Fact]
    public async Task GetBookCategoryById_ShouldThrow_WhenCategoryNotFound()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryById(It.IsAny<Guid>())).ReturnsAsync((BookCategory?)null);

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
        var created = BookCategory.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryById(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new BookCategoryDto { Name = "B" });

        //assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("B");

        _repo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenCategoryNotFound()
    {
        //arrange
        _repo.Setup(r => r.GetBookCategoryById(It.IsAny<Guid>())).ReturnsAsync((BookCategory?)null);

        //act
        var result = await _service.UpdateBookCategoryAsync(Guid.NewGuid(), new BookCategoryDto { Name = "B" });

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category not found.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenNameIsEmpty()
    {
        //arrange
        var created = BookCategory.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryById(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new BookCategoryDto { Name = ""});

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name is required.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public async Task UpdateBookCategoryAsync_ShouldReturnFailure_WhenNameTooLong()
    {
        //arrange
        var created = BookCategory.Create("A");
        created.IsSuccess.Should().BeTrue();

        var category = created.Value!;

        _repo.Setup(r => r.GetBookCategoryById(category.Id)).ReturnsAsync(category);

        //act
        var result = await _service.UpdateBookCategoryAsync(category.Id, new BookCategoryDto { Name = new string('A', 101) });

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category name cannot exceed 100 characters.");

        _repo.Verify(r => r.SaveChanges(), Times.Never());
    }
}