using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Application.Books.DTOs;
public class BookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public string Content { get; set; }
    public int PublishedYear { get; set; }
}
