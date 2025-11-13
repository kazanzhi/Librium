using Librium.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Librium.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserBookCategoryController : ControllerBase
{
    private readonly IUserBookService _service;
    public UserBookCategoryController(IUserBookService service)
    {
        _service = service;
    }
}
