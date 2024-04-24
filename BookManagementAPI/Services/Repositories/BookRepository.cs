using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Book GetBook(string title)
        {
            return _context.Books.Include(g => g.Genre).SingleOrDefault(x => x.Title == title);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void RemoveBook(Guid id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
    }
}
