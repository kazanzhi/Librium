using FluentAssertions;
using Librium.Application.DTOs.Books;
using Librium.Application.Services.Books;
using Librium.Domain.Books;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Categories;
using Librium.Domain.Categories.Repositories;
using Moq;

namespace Librium.Tests.Application;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepo;
    private readonly Mock<ICategoryRepository> _categoryRepo;
    private readonly BookService _service;
    public BookServiceTests()
    {
        _bookRepo = new Mock<IBookRepository>();
        _categoryRepo = new Mock<ICategoryRepository>();
        _service = new BookService(_bookRepo.Object, _categoryRepo.Object);
    }

    //create
    [Fact]
    public async Task CreateBookAsync_ShouldCallAddBookAndSaveChanges_WhenBookCreated()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "TestTitle",
            Author = "TestAuthor",
            Content = "TestContent",
            PublishedYear = 2000
        };

        _bookRepo.Setup(r => r.ExistBookAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Once());
        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task CreateBookAsync_ShouldReturnFailure_WhenBookWithSameAuthorAndTitleExists()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "TestTitle",
            Author = "TestAuthor",
            Content = "TestContent",
            PublishedYear = 2000
        };

        var book = Book.Create("TestTitle", "TestAuthor", "TestContent", 1999).Value!;

        _bookRepo.Setup(r => r.ExistBookAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("A book with the same author and title already exsits.");

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldReturnFailure_WhenDomainCreateReturnsFailure()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "",
            Author = "TestAuthor",
            Content = "TestContent",
            PublishedYear = 2000
        };

        _bookRepo.Setup(r => r.ExistBookAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    //delete
    [Fact]
    public async Task DeleteBookAsync_ShouldReturnSuccess_WhenBookDeleted()
    {
        //arrange
        var book = Book.Create("TestTitle", "TestAuthor", "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        //act
        var result = await _service.DeleteBookAsync(book.Id);

        //assert
        result.IsSuccess.Should().BeTrue();

        _bookRepo.Verify(r => r.Delete(book), Times.Once());
        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task DeleteBookAsync_ShouldReturnFailure_WhenBookNotFound()
    {
        //arrange
        _bookRepo.Setup(r => r.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

        //act
        var result = await _service.DeleteBookAsync(Guid.NewGuid());

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Book not found.");

        _bookRepo.Verify(r => r.Delete(It.IsAny<Book>()), Times.Never);
        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    //getAll
    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnMappedList()
    {
        //arrange
        var book1 = Book.Create("TestTitle", "TestAuthor", "TestContent", 2000).Value!;
        var book2 = Book.Create("NewTitle", "NewtAuthor", "NewContent", 2025).Value!;

        _bookRepo.Setup(r => r.GetAllBooks(null)).ReturnsAsync(new List<Book> { book1, book2 });

        //act
        var result = await _service.GetAllBooksAsync(null);

        //assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(book1.Id);
        result[0].Title.Should().Be(book1.Title);
        result[0].Author.Should().Be(book1.Author);
        result[0].Content.Should().Be(book1.Content);
        result[0].PublishedYear.Should().Be(book1.PublishedYear);

        result[1].Id.Should().Be(book2.Id);
        result[1].Title.Should().Be(book2.Title);
        result[1].Author.Should().Be(book2.Author);
        result[1].Content.Should().Be(book2.Content);
        result[1].PublishedYear.Should().Be(book2.PublishedYear);
    }

    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnEmptyList_WhenNoBooks()
    {
        //arrange
        _bookRepo.Setup(r => r.GetAllBooks(null)).ReturnsAsync(new List<Book>());

        //act
        var result = await _service.GetAllBooksAsync(null);

        //assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllBooksAsync_ShouldPassSearchToRepository()
    {
        //arrange
        var search = "test";

        _bookRepo.Setup(r => r.GetAllBooks(search)).ReturnsAsync(new List<Book>());

        //act
        var result = await _service.GetAllBooksAsync(search);

        //assert
        _bookRepo.Verify(r => r.GetAllBooks(search), Times.Once);
    }

    //getById
    [Fact]
    public async Task GetBookById_ShouldReturnMappedDto()
    {
        // arrange
        var book = Book.Create("TestTitle", "TestAuthor", "OldContent", 1999).Value!;

        _bookRepo
            .Setup(r => r.GetBookById(book.Id))
            .ReturnsAsync(book);

        // act
        var result = await _service.GetBookById(book.Id);

        // assert
        result.IsSuccess.Should().BeTrue();

        var dto = result.Value!;

        dto.Id.Should().Be(book.Id);
        dto.Title.Should().Be(book.Title);
        dto.Author.Should().Be(book.Author);
        dto.Content.Should().Be(book.Content);
        dto.PublishedYear.Should().Be(book.PublishedYear);
    }


    [Fact]
    public async Task GetBookById_ShouldReturnFailure_WhenBookNotFound()
    {
        _bookRepo.Setup(r => r.GetBookById(It.IsAny<Guid>()))
            .ReturnsAsync((Book?)null);

        var result = await _service.GetBookById(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Book not found.");
    }

    //update
    [Fact]
    public async Task UpdateBookAsync_ShouldReturnSuccess_WhenBookUpdated()
    {
        //act
        var dto = new BookDto
        {
            Title = "NewTitle",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2000
        };

        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        //arrange
        var result = await _service.UpdateBookAsync(book.Id, dto);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be(dto.Title);
        book.Author.Should().Be(dto.Author);
        book.Content.Should().Be(dto.Content);
        book.PublishedYear.Should().Be(dto.PublishedYear);

        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldReturnFailure_WhenBookNotFound()
    {
        //arrangeчё
        var dto = new BookDto
        {
            Title = "NewTitle",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2000
        };
        var bookId = Guid.NewGuid();

        _bookRepo.Setup(r => r.GetBookById(bookId)).ReturnsAsync((Book?)null);

        //act
        var result = await _service.UpdateBookAsync(bookId, dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Book not found.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldReturnFailure_WhenDomainUpdateReturnFailure()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "",
            Author = "NewAuthor",
            Content = "NewContent",
            PublishedYear = 2000
        };

        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        //act
        var result = await _service.UpdateBookAsync(book.Id, dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    //AddCategoryToBook
    [Fact]
    public async Task AddCategoryToBook_ShouldReturnSuccess_WhenCategoryAdded()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;
        var category = Category.Create("Education").Value;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetByNameAsync("Education")).ReturnsAsync(category);

        //act
        var result = await _service.AddCategoryToBook(book.Id, "Education");

        //assert
        result.IsSuccess.Should().BeTrue();

        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task AddCategoryToBook_ShouldReturnFailure_WhenBookNotFound()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync((Book)null);

        //act
        var result = await _service.AddCategoryToBook(Guid.NewGuid(), "Education");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Book not found.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never());
    }


    [Fact]
    public async Task AddCategoryToBook_ShouldReturnFailyre_WhenCategoryDoesNotExists()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetByNameAsync("Education")).ReturnsAsync((Category)null);

        //act
        var result = await _service.AddCategoryToBook(book.Id, "Education");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Category Education does not exists.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never());
    }

    //RemoveCategoryFromBook
    [Fact]
    public async Task RemoveCategoryFromBook_ShouldReturnSuccess_WhenCategoryAdded()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;
        var category = Category.Create("Education").Value;
        book.AddCategory(category);

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetByNameAsync("Education")).ReturnsAsync(category);

        //act
        var result = await _service.RemoveCategoryFromBook(book.Id, "Education");

        //assert
        result.IsSuccess.Should().BeTrue();

        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task RemoveCategoryFromBook_ShouldReturnFailure_WhenBookNotFound()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync((Book)null);

        //act
        var result = await _service.RemoveCategoryFromBook(Guid.NewGuid(), "Education");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Book not found.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never());
    }


    [Fact]
    public async Task RemoveCategoryFromBook_ShouldReturnFailyre_WhenCategoryDoesNotExists()
    {
        //arrange
        var book = Book.Create("OldTitle", "OldAuthor", "OldContent", 1999).Value;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetByNameAsync("Education")).ReturnsAsync((Category)null);

        //act
        var result = await _service.RemoveCategoryFromBook(book.Id, "Education");

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Category Education does not exists.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never());
    }
}