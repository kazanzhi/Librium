using FluentAssertions;
using Librium.Application.Books.Commands.AddCategoryToBook;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application.Books.Commands;

public class AddCategoryToBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSucceed_WhenCategoryAdded()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var category = Category.Create("Category").Value!;

        var categoryRepoMock = new Mock<ICategoryRepository>();
        var bookRepoMock = new Mock<IBookRepository>();
        categoryRepoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        bookRepoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new AddCategoryToBookCommandHandler(bookRepoMock.Object, categoryRepoMock.Object);
        var command = new AddCategoryToBookCommand(book.Id, category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        bookRepoMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        categoryRepoMock
            .Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        bookRepoMock
            .Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBookDoesNotExist()
    {
        //arrange
        var bookId = Guid.NewGuid();
        var category = Category.Create("Category").Value!;

        var categoryRepoMock = new Mock<ICategoryRepository>();
        var bookRepoMock = new Mock<IBookRepository>();
        categoryRepoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        bookRepoMock
            .Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        var handler = new AddCategoryToBookCommandHandler(bookRepoMock.Object, categoryRepoMock.Object);
        var command = new AddCategoryToBookCommand(bookId, category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        bookRepoMock
            .Verify(r => r.GetBookById(bookId), Times.Once);
        categoryRepoMock
            .Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Never);
        bookRepoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryDoesNotExist()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var categoryId = Guid.NewGuid();

        var categoryRepoMock = new Mock<ICategoryRepository>();
        var bookRepoMock = new Mock<IBookRepository>();
        categoryRepoMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category?)null);
        bookRepoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new AddCategoryToBookCommandHandler(bookRepoMock.Object, categoryRepoMock.Object);
        var command = new AddCategoryToBookCommand(book.Id, categoryId);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        bookRepoMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        categoryRepoMock
            .Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
        bookRepoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryIsAlreadyAssigned()
    {
        //arrange
        var book = Book.Create("Title", "Author", "Content", 2000).Value!;
        var category = Category.Create("Category").Value!;
        book.AddCategory(category);

        var categoryRepoMock = new Mock<ICategoryRepository>();
        var bookRepoMock = new Mock<IBookRepository>();
        categoryRepoMock
            .Setup(r => r.GetCategoryByIdAsync(category.Id)).ReturnsAsync(category);
        bookRepoMock
            .Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        var handler = new AddCategoryToBookCommandHandler(bookRepoMock.Object, categoryRepoMock.Object);
        var command = new AddCategoryToBookCommand(book.Id, category.Id);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeEmpty();

        bookRepoMock
            .Verify(r => r.GetBookById(book.Id), Times.Once);
        categoryRepoMock
            .Verify(r => r.GetCategoryByIdAsync(category.Id), Times.Once);
        bookRepoMock
            .Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}