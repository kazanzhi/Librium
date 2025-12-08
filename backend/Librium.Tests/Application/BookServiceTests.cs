using FluentAssertions;
using Librium.Application.Services;
using Librium.Domain.Books.DTOs;
using Librium.Domain.Books.Models;
using Librium.Domain.Books.Repositories;
using Librium.Domain.DTOs.Books;
using Moq;

namespace Librium.Tests.Application;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepo;
    private readonly Mock<IBookCategoryRepository> _categoryRepo;
    private readonly BookService _service;
    public BookServiceTests()
    {
        _bookRepo = new Mock<IBookRepository>();
        _categoryRepo = new Mock<IBookCategoryRepository>();
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
            Category = "Science", 
            Content = "TestContent", 
            PublishedYear = 2000
        };

        var category = BookCategory.Create("Science").Value!;
        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book>());
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Once());
        _bookRepo.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public async Task CreateBookAsync_ShouldTrimTitleAndAuthor_WhenCheckingDuplicates()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "   TestTitle   ",
            Author = "  TestAuthor  ",
            Category = "Science",
            Content = "Content",
            PublishedYear = 2000
        };

        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("TestTitle", "TestAuthor", category.Id, "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book> { book });
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("A book with the same author and title already exsits.");

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldReturnSuccess_WhenCategoryNameTrimmed()
    {
        //arrange
        var dto = new BookDto
        {
            Title = "TestTitle",
            Author = "TestAuthor",
            Category = "   Science  ",
            Content = "TestContent",
            PublishedYear = 2000
        };
        var category = BookCategory.Create("Science").Value!;

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book>());
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

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
            Category = "Science", 
            Content = "TestContent", 
            PublishedYear = 2000 
        };

        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("TestTitle", "TestAuthor", category.Id, "TestContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book> { book });
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("A book with the same author and title already exsits.");

        _bookRepo.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
    {
        //arrange
        var dto = new BookDto 
        { 
            Title = "TestTitle", 
            Author = "TestAuthor", 
            Category = "Science", 
            Content = "TestContent", 
            PublishedYear = 2000 
        };

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book>());
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

        //act
        var result = await _service.CreateBookAsync(dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Category {dto.Category} does not exists.");

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
            Category = "Science",
            Content = "TestContent",
            PublishedYear = 2000
        };
        var category = BookCategory.Create("Science").Value!;

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book>());
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

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
        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("TestTitle", "TestAuthor", category.Id, "OldContent", 1999).Value!;

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
        var category = BookCategory.Create("Test").Value!;
        var book1 = Book.Create("TestTitle", "TestAuthor", category.Id, "TestContent", 2000).Value!;
        var book2 = Book.Create("NewTitle", "NewtAuthor", category.Id, "NewContent", 2025).Value!;

        book1.BookCategory = category;
        book2.BookCategory = category;

        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book> { book1, book2 });

        //act
        var result = await _service.GetAllBooksAsync();

        //assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(book1.Id);
        result[0].Title.Should().Be(book1.Title);
        result[0].Author.Should().Be(book1.Author);
        result[0].Content.Should().Be(book1.Content);
        result[0].PublishedYear.Should().Be(book1.PublishedYear);
        result[0].BookCategory.Should().Be(category.Name);

        result[1].Id.Should().Be(book2.Id);
        result[1].Title.Should().Be(book2.Title);
        result[1].Author.Should().Be(book2.Author);
        result[1].Content.Should().Be(book2.Content);
        result[1].PublishedYear.Should().Be(book2.PublishedYear);
        result[1].BookCategory.Should().Be(category.Name);
    }

    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnEmptyList_WhenNoBooks()
    {
        //arrange
        _bookRepo.Setup(r => r.GetAllBooks()).ReturnsAsync(new List<Book>());

        //act
        var result = await _service.GetAllBooksAsync();

        //assert
        result.Should().BeEmpty();
    }

    //getById
    [Fact]
    public async Task GetBookById_ShouldReturnMappedDto()
    {
        //arrange
        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("TestTitle", "TestAuthor", category.Id, "OldContent", 1999).Value!;
        book.BookCategory = category;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);

        //act
        var result = await _service.GetBookById(book.Id);

        //assert
        result.Should().BeOfType<BookResponseDto>();

        result.Id.Should().Be(book.Id);
        result.Title.Should().Be(book.Title);
        result.Author.Should().Be(book.Author);
        result.Content.Should().Be(book.Content);
        result.PublishedYear.Should().Be(book.PublishedYear);
        result.BookCategory.Should().Be(category.Name);
    }

    [Fact]
    public async Task GetBookById_ShouldThrow_WhenBookNotFound()
    {
        //arrange
        _bookRepo.Setup(r => r.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

        //act
        var result = async() => await _service.GetBookById(Guid.NewGuid());

        //assert
        await result.Should().ThrowAsync<NullReferenceException>();
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
            Category = "Science",
            Content = "NewContent",
            PublishedYear = 2000
        };

        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("OldTitle", "OldAuthor", category.Id, "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

        //arrange
        var result = await _service.UpdateBookAsync(book.Id, dto);

        //assert
        result.IsSuccess.Should().BeTrue();
        book.Title.Should().Be(dto.Title);
        book.Author.Should().Be(dto.Author);
        book.Content.Should().Be(dto.Content);
        book.PublishedYear.Should().Be(dto.PublishedYear);
        book.BookCategory.Should().Be(category);


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
            Category = "Science",
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
    public async Task UpdateBookAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
    {
        //act
        var dto = new BookDto
        {
            Title = "NewTitle",
            Author = "NewAuthor",
            Category = "Education",
            Content = "NewContent",
            PublishedYear = 2000
        };

        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("OldTitle", "OldAuthor", category.Id, "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory>());

        //arrange
        var result = await _service.UpdateBookAsync(book.Id, dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Category does not exist.");

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
            Category = "Science",
            Content = "NewContent",
            PublishedYear = 2000
        };

        var category = BookCategory.Create("Science").Value!;
        var book = Book.Create("OldTitle", "OldAuthor", category.Id, "OldContent", 1999).Value!;

        _bookRepo.Setup(r => r.GetBookById(book.Id)).ReturnsAsync(book);
        _categoryRepo.Setup(r => r.GetAllBookCategories()).ReturnsAsync(new List<BookCategory> { category });

        //act
        var result = await _service.UpdateBookAsync(book.Id, dto);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Title is required.");

        _bookRepo.Verify(r => r.SaveChanges(), Times.Never);
    }
}