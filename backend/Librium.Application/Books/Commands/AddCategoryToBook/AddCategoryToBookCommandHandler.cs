using Librium.Domain.Books.Repositories;
using Librium.Domain.Categories.Repositories;
using Librium.Domain.Common;
using MediatR;

namespace Librium.Application.Books.Commands.AddCategoryToBook;

public sealed class AddCategoryToBookCommandHandler : IRequestHandler<AddCategoryToBookCommand, ValueOrResult>
{
    private readonly IBookRepository _bookRepo;
    private readonly ICategoryRepository _categoryRepo;
    public AddCategoryToBookCommandHandler(IBookRepository bookRepo, ICategoryRepository categoryRepo)
    {
        _bookRepo = bookRepo;
        _categoryRepo = categoryRepo;
    }
    public async Task<ValueOrResult> Handle(AddCategoryToBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepo.GetBookById(request.BookId);
        if (book is null)
            return ValueOrResult.Failure("Book not found.");

        var category = await _categoryRepo.GetBookCategoryByIdAsync(request.CategoryId);
        if (category is null)
            return ValueOrResult.Failure($"Category not found.");

        var addResult = book.AddCategory(category);
        if (!addResult.IsSuccess)
            return ValueOrResult.Failure(addResult.ErrorMessage!);

        await _bookRepo.SaveChanges();

        return ValueOrResult.Success();
    }
}
