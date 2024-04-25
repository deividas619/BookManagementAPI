using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController(IBookService service) : ControllerBase
{
    [HttpGet("GetAllBooks")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
    {
        var result = await service.GetAllBooks();
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

        var result = await service.GetBooksByTitle(title);

        if (!result.Any())
            return BadRequest("There are no books matching the title!");
        return Ok(result);
    }

    [HttpGet("GetBookById")]
    [AllowAnonymous]
    public async Task<ActionResult<Book>> GetBookById(Guid id)
    {
        var result = await service.GetBookById(id);
        ;

        if (result is null)
            return BadRequest("There are no books matching the id!");
        return Ok(result);
    }

    [HttpPost("AddBook")]
    [Authorize(Roles = "Regular")]
    public async Task<ActionResult<Book>> AddBook([FromBody] BookDto book)
    {
        var result = await service.AddBook(book.Title, book.Author, book.Publication, book.Genre);

        if (result is null)
            return BadRequest("Failed to add a book!");
        return Ok(result);
    }

    [HttpPut("UpdateBook")]
    [Authorize(Roles = "Admin, Regular")]
    public async Task<ActionResult<Book>> UpdateBook([FromBody] Book currentBook)
    {
        var result = await service.UpdateBook(currentBook);

        if (result is null)
            return BadRequest("Failed to update a book!");
        return Ok(result);
    }

    [HttpDelete("RemoveBookById")]
    [Authorize(Roles = "Admin, Regular")]
    public async Task<ActionResult<Book>> RemoveBookById([FromQuery] Guid id)
    {
        var result = await service.RemoveBookById(id);

        if (result is null)
            return BadRequest("Failed to delete a book!");
        return Ok(result);
    }
}