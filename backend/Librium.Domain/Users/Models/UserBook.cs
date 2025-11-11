using Librium.Domain.Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Users.Models;
public class UserBook
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime AddedAt { get; set; }
}
