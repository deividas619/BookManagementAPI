using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using System.Threading.Tasks;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService service)
        {
            _bookService = service;
        }

        [HttpGet("GetAllBooks")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var result = await _bookService.GetAllBooks();
            if (!result.Any())
                return BadRequest("There are no books in the database!");
            return Ok(result);
        }

        [HttpGet("GetBooksByTitle")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByTitle(string title)
        {
            if (title is null)
                return BadRequest("Book title was not provided!");

            var result = await _bookService.GetBooksByTitle(title);

            if (!result.Any())
                return BadRequest("There are no books matching the title!");
            return Ok(result);
        }

        [HttpGet("GetBookById")]
        [AllowAnonymous]
        public async Task<ActionResult<Book>> GetBookById(Guid id)
        {
            var result = await _bookService.GetBookById(id); ;

            if (result is null)
                return BadRequest("There are no books matching the id!");
            return Ok(result);
        }

        [HttpPost("AddBook")]
        [Authorize(Roles = "Regular")]
        public async Task<ActionResult<Book>> AddBook([FromBody] BookDto book)
        {
            var result = await _bookService.AddBook(book.Title, book.Author, book.Publication, book.Genre);
            
            if (result is null)
                return BadRequest("Failed to add a book!");
            return Ok(result);
        }

        [HttpPut("UpdateBook")]
        [Authorize(Roles = "Admin, Regular")]
        public async Task<ActionResult<Book>> UpdateBook([FromBody] Book currentBook)
        {
            var result = await _bookService.UpdateBook(currentBook);
            
            if (result is null)
                return BadRequest("Failed to update a book!");
            return Ok(result);
        }

        [HttpDelete("RemoveBookById")]
        [Authorize(Roles = "Admin, Regular")]
        public async Task<ActionResult<Book>> RemoveBookById([FromQuery] Guid id)
        {
            var result = await _bookService.RemoveBookById(id);
            
            if (result is null)
                return BadRequest("Failed to delete a book!");
            return Ok(result);
        }
    }
}
