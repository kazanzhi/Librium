using FluentAssertions;
using Librium.Application.Books.Commands.RemoveCategoryFromBook;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Commands;

public class RemoveCategoryFromBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCategoryIsAssignedToBook()
    {
        //arrange
        var category = Category.Create("Category").Value!;
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        book.AddCategory(category);

        var repoCategoryMock = new Mock<ICategoryRepository>();
        var repoBookMock = new Mock<IBookRepository>();

        repoCategoryMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        repoBookMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new RemoveCategoryFromBookCommandHandler(
            repoBookMock.Object, 
            repoCategoryMock.Object
        );
        var command = new RemoveCategoryFromBookCommand(book.Id, category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        repoBookMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        repoCategoryMock
            .Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        repoBookMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookDoesNotExist()
    {
        //arrange
        var bookId = Guid.NewGuid();
        var category = Category.Create("Category").Value!;

        var repoCategoryMock = new Mock<ICategoryRepository>();
        var repoBookMock = new Mock<IBookRepository>();

        repoCategoryMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        repoBookMock
            .Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        var handler = new RemoveCategoryFromBookCommandHandler(
            repoBookMock.Object, 
            repoCategoryMock.Object
        );
        var command = new RemoveCategoryFromBookCommand(bookId, category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoBookMock
            .Verify(r => r.GetBookById(bookId), Times.Once);
        repoCategoryMock
            .Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Never);
        repoBookMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryDoesNotExist()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var categoryId = Guid.NewGuid();

        var repoCategoryMock = new Mock<ICategoryRepository>();
        var repoBookMock = new Mock<IBookRepository>();

        repoCategoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category?)null);
        repoBookMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new RemoveCategoryFromBookCommandHandler(
            repoBookMock.Object, 
            repoCategoryMock.Object
        );
        var command = new RemoveCategoryFromBookCommand(book.Id, categoryId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoBookMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        repoCategoryMock
            .Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
        repoBookMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryIsNotAssignedToBook()
    {
        // arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var category = Category.Create("Category").Value!;

        var repoCategoryMock = new Mock<ICategoryRepository>();
        var repoBookMock = new Mock<IBookRepository>();

        repoCategoryMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        repoBookMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new RemoveCategoryFromBookCommandHandler(
            repoBookMock.Object,
            repoCategoryMock.Object
        );

        var command = new RemoveCategoryFromBookCommand(book.Id, category.Id);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        repoBookMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

}