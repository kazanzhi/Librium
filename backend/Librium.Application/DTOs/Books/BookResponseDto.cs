using Librium.Domain.Entities.Books;
using Librium.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Application.DTOs.Books;

public class BookResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string BookCategory { get; set; }
    public string Content { get; set; }
    public int PublishedYear { get; set; }
}
