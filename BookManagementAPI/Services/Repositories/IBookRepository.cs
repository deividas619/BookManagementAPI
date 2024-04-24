using System;
using System.Collections.Generic;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        Book GetBookByTitle(string title);
        Book GetBookById(Guid id);
        void AddBook(Book book);
        void RemoveBook(Guid id);
    }
}
