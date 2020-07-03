using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using noesis_api.Contexts;
using noesis_api.Models;
using noesis_api.DTOs;
using noesis_api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace noesis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly NoesisApiContext _context;
        private readonly IBookService _bookService;
        private readonly ICommentService _commentService;
        private readonly INoteService _noteService;

        public BooksController(
            NoesisApiContext context,
            IBookService bookService,
            ICommentService commentService,
            INoteService noteService
        )
        {
            _context = context;
            _bookService = bookService;
            _commentService = commentService;
            _noteService = noteService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] int page = 0)
        {
            var results = await _bookService.GetPageOfBooks(page);
            return Ok(results);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(long id)
        {
            var book = await _bookService.GetBookDetails(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, BookDetailDTO book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var updatedBook = await _bookService.UpdateBook(book);

            return Ok(updatedBook);
        }

        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> PostBook(BookDetailDTO book)
        {
            var newBook = await _bookService.AddBook(book);

            return Ok(newBook);

        }

        // DELETE: api/Books/5
        [Authorize(Roles = UserRole.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var result = await _bookService.DeleteBook(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{bookId}/comments")]
        public async Task<IActionResult> PostComment(long bookId, Comment comment)
        {
            long userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!BookExists(bookId))
            {
                return BadRequest();
            }

            var result = await _commentService.AddComment(userId, bookId, comment);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{bookId}/comments/{commentId}")]
        public async Task<IActionResult> PutComment(long bookId, long commentId, Comment comment)
        {
            // Authorize: check if user submitting is the same as author of the comment
            UserComment userComment = await _context.UserComments.FirstOrDefaultAsync(uc => uc.CommentId == commentId);

            if (userComment == null)
            {
                return BadRequest();
            }
            if (userComment.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var updatedComment = await _commentService.UpdateComment(commentId, comment);

            return Ok(updatedComment);
        }

        [Authorize]
        [HttpDelete("{bookId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(long bookId, long commentId)
        {
            UserComment userComment = await _context.UserComments.FirstOrDefaultAsync(uc => uc.CommentId == commentId);

            if (userComment == null)
            {
                return BadRequest();
            }
            if (userComment.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var removedComment = await _commentService.DeleteComment(commentId, userComment);

            return Ok(removedComment);
        }

        [Authorize]
        [HttpPost("{bookId}/notes")]
        public async Task<IActionResult> PostNote(long bookId, Note note)
        {
            long userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!BookExists(bookId))
            {
                return BadRequest();
            }

            var result = await _noteService.AddNote(userId, bookId, note);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{bookId}/notes/{noteId}")]
        public async Task<IActionResult> PutNote(long bookId, long noteId, Note note)
        {
            UserNote userNote = await _context.UserNotes.FirstOrDefaultAsync(un => un.NoteId == noteId);

            if (userNote == null)
            {
                return BadRequest();
            }
            if (userNote.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var updatedNote = await _noteService.UpdateNote(noteId, note);

            return Ok(updatedNote);
        }

        [Authorize]
        [HttpDelete("{bookId}/notes/{noteId}")]
        public async Task<IActionResult> DeleteNote(long bookId, long noteId)
        {
            UserNote userNote = await _context.UserNotes.FirstOrDefaultAsync(un => un.NoteId == noteId);

            if (userNote == null)
            {
                return BadRequest();
            }
            if (userNote.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var deletedNote = await _noteService.DeleteNote(noteId, userNote);

            return Ok(deletedNote);
        }

        [Authorize]
        [HttpPost("{bookId}/ratings")]
        public async Task<IActionResult> PostRating(long bookId, UserRating userRating)
        {
            var rating = await _bookService.AddRating(bookId, userRating);

            return Ok(rating);
        }

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
