using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet("GetBook")]
        [AllowAnonymous]
        public ActionResult<Book> GetBooks([FromQuery] string title)
        {
            return _service.GetBook(title);
        }

        [HttpDelete("Remove")]
        [Authorize(Roles = "Admin")]
        public void RemoveBook([FromQuery] Guid id)
        {
            _service.RemoveBook(id);
        }

        [HttpPost("AddBook")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ResponseDto> AddBook(Book book)
        {
            return _service.AddBook(book);
        }
    }
}
