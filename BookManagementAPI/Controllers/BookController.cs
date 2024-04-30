using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

    [HttpPost("GetBooksByFilter")]
    [AllowAnonymous]
    public async Task<ActionResult<Book>> GetBooksByFilter(string? title, string? author, string[] genres, DateOnly? publicationAfterDate, DateOnly? publicationBeforeDate)
    {
        var searchFilter = new SearchFilterDto
        {
            Title = title,
            Author = author,
            Genres = genres,
            PublicationAfterDate = publicationAfterDate,
            PublicationBeforeDate = publicationBeforeDate
        };
        var result = await service.GetBooksByFilter(searchFilter, 0, 0);

        if (result is null)
            return BadRequest("There are no books matching the id!");
        return Ok(result);
    }

    [HttpPost("AddBook")]
    //[Authorize(Roles = "Admin, Regular")] //Augustas: commented due to user roles enum
    [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.Regular))] //Augustas: user roles enum
    public async Task<ActionResult<Book>> AddBook([FromBody] BookDto book)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var result = await service.AddBook(book.Title, book.Author, book.Publication, book.Genre, userName);

        if (result is null)
            return BadRequest("Failed to add a book!");
        return Ok(result);
    }

    [HttpPut("UpdateBook/{id}")]
    //[Authorize(Roles = "Admin, Regular")] //Augustas: commented due to user roles enum
    [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.Regular))] //Augustas: user roles enum
    public async Task<ActionResult<Book>> UpdateBook([FromRoute]Guid id, [FromBody] BookDto currentBook)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var userNameRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        var result = await service.UpdateBook(id, currentBook, userName, userNameRole);

        if (result is null)
            return BadRequest("Failed to update a book!");
        return Ok(result);
    }

    [HttpDelete("RemoveBookById")]
    //[Authorize(Roles = "Admin, Regular")] //Augustas: commented due to user roles enum
    [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.Regular))] //Augustas: user roles enum
    public async Task<ActionResult<Book>> RemoveBookById([FromQuery] Guid id)
    {
        var result = await service.RemoveBookById(id);

        if (result is null)
            return BadRequest("Failed to delete a book!");
        return Ok(result);
    }
}