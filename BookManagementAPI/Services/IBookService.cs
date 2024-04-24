using System;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public interface IBookService
    {
        ResponseDto AddBook(Book book);
        Book GetBook(string title);
        void RemoveBook(Guid id);
    }
}
