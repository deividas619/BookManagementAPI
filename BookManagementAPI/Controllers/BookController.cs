using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;

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
        public List<Book> GetAllBooks()
        {
            return _bookService.GetAllBooks();
        }

        [HttpGet("GetBookByTitle")]
        [AllowAnonymous]
        public Book GetBookByTitle(string title)
        {
            return _bookService.GetBookByTitle(title);
        }

        [HttpPost("AddBook")]
        [Authorize(Roles = "Regular")]
        public ActionResult<ResponseDto> AddBook([FromBody] BookDto book)
        {
            var response = _bookService.AddBook(book.Title, book.Author, book.Publication, book.Genre);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return response;
        }

        [HttpDelete("RemoveBook")]
        [Authorize(Roles = "Admin, Regular")]
        public ActionResult<ResponseDto> RemoveBook([FromQuery] Guid id)
        {
            var response = _bookService.RemoveBook(id);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return response;
        }
    }
}
