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

namespace noesis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly NoesisApiContext _context;
        private readonly IBookService _bookService;

        public BooksController(NoesisApiContext context, IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
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
        [HttpPost]
        public async Task<IActionResult> PostBook(BookDetailDTO book)
        {
            var newBook = await _bookService.AddBook(book);

            return Ok(newBook);

        }

        // DELETE: api/Books/5
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

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
