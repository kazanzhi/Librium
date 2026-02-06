    using Librium.Application.Books.DTOs;
    using Librium.Application.Categories.DTOs;
    using Librium.Domain.Books.Repositories;
    using Librium.Domain.Common;
    using MediatR;

    namespace Librium.Application.Books.Queries.GetBookById;

    public sealed class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, ValueOrResult<BookResponseDto>>
    {
        private readonly IBookRepository _repo;
        public GetBookByIdQueryHandler(IBookRepository repo)
        {
            _repo = repo;
        }
        public async Task<ValueOrResult<BookResponseDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _repo.GetBookById(request.Id);
            if (book is null)
                return ValueOrResult<BookResponseDto>.Failure("Book not found.");

            var dto = new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Categories = book.Categories
                    .Select(c => new CategoryResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList(),
                Content = book.Content,
                PublishedYear = book.PublishedYear
            };

            return ValueOrResult<BookResponseDto>.Success(dto);
        }
    }
