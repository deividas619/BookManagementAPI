using System;
using System.Collections.Generic;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public interface IBookService
    {
        ResponseDto AddBook(string title, string author, DateOnly publication, Genre genre);
        List<Book> GetAllBooks();
        Book GetBookByTitle(string title);
        ResponseDto RemoveBook(Guid id);
    }
}
