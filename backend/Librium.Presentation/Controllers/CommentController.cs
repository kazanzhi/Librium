using Librium.Application.Comments.Commands.CreateComment;
using Librium.Application.Comments.Commands.DeleteComment;
using Librium.Application.Comments.Commands.ReactToComment;
using Librium.Application.Comments.Commands.UpdateComment;
using Librium.Application.Comments.DTOs;
using Librium.Application.Comments.Queries.GetCommentById;
using Librium.Application.Comments.Queries.GetCommentsForBook;
using Librium.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Librium.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ISender _sender;
        public CommentController(ISender sender)
        {
            _sender = sender;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
        }

        [HttpPost("books/{bookId:guid}/comments")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Create([FromBody] CommentDto dto, Guid bookId)
        {
            var userId = GetUserId();
            var result = await _sender.Send(new CreateCommentCommand(userId, bookId, dto));

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { commentId = result.Value }, result.Value)
                : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("comments/{commentId:guid}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Delete(Guid commentId)
        {
            var userId = GetUserId();
            var result = await _sender.Send(new DeleteCommentCommand(commentId, userId));

            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.ErrorMessage);
        }

        [HttpGet("comments/{commentId:guid}")]
        public async Task<IActionResult> GetById(Guid commentId)
        {
            var result = await _sender.Send(new GetCommentByIdQuery(commentId));

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.ErrorMessage);
        }

        [HttpGet("books/{bookId:guid}/comments")]
        public async Task<IActionResult> GetForBook(Guid bookId)
        {
            var comments = await _sender.Send(new GetCommentsForBookQuery(bookId));
            return Ok(comments);
        }

        [HttpPut("comments/{commentId:guid}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Update(Guid commentId, [FromBody] CommentDto dto)
        {
            var userId = GetUserId();
            var updateResult = await _sender.Send(new UpdateCommentCommand(commentId, userId, dto));

            return updateResult.IsSuccess
                ? NoContent()
                : BadRequest(updateResult.ErrorMessage);
        }

        [HttpPost("comments/{commentId:guid}/reaction")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Reaction(Guid commentId, [FromBody] ReactToCommentRequest request)
        {
            var userId = GetUserId();
            var reacttionResult = await _sender.Send(new ReactToCommentCommand(commentId, userId, request.ReactionType));

            return reacttionResult.IsSuccess
                ? NoContent()
                : BadRequest(reacttionResult.ErrorMessage);
        }
    }
}
