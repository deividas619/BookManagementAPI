using System;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public interface IBookRepository
    {
        Book GetBook(string title);
        void AddBook(Book book);
        void RemoveBook(Guid id);
    }
}
