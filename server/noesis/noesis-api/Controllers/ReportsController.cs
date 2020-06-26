using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using noesis_api.Contexts;
using noesis_api.Models;
using noesis_api.DTOs;
using noesis_api.Services;

namespace noesis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IBookService _bookService;

        public ReportsController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("topcategories")]
        public async Task<IActionResult> TopCategories()
        {
            var results = await _bookService.GetTopCategories();

            return Ok(results);
        }

        [HttpGet("topcategories/{id}")]
        public async Task<IActionResult> TopBooksInCategory(long id)
        {
            var results = await _bookService.TopInCategory(id);

            return Ok(results);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string terms = null)
        {
            if (terms != null)
            {
                var results = await _bookService.SearchBooks(terms);

                return Ok(results);
            }

            return BadRequest("You must have search terms!");
        }

        [HttpGet("allcategories")]
        public async Task<IActionResult> AllCategories()
        {
            var results = await _bookService.GetAllCategories();

            return Ok(results);
        }

        [HttpGet("allauthors")]
        public async Task<IActionResult> AllAuthors()
        {
            var results = await _bookService.GetAllAuthors();

            return Ok(results);
        }
    }
}