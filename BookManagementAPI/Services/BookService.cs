using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;

namespace BookManagementAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository repository)
        {
            _bookRepository = repository;
        }

        public ResponseDto AddBook(string title, string author, DateOnly publication, Genre genre)
        {
            var existingBook = _bookRepository.GetAllBooks().FirstOrDefault(b => b.Title == title && b.Author == author && b.Publication == publication);
            if (existingBook is not null)
                return new ResponseDto(false, "Book already exist");

            var newBook = CreateBook(title, author, publication, genre);
            _bookRepository.AddBook(newBook);
            return new ResponseDto(true, "Book added successfully!");
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBookByTitle(string title)
        {
            return _bookRepository.GetBookByTitle(title);
        }

        public ResponseDto RemoveBook(Guid id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book is null)
                return new ResponseDto(false, "Book doesn't exist!");
            
            _bookRepository.RemoveBook(id);
            return new ResponseDto(true, "Book successfully removed!");
        }

        private Book CreateBook(string title, string author, DateOnly publication, Genre genre)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Author = author,
                Publication = publication,
                Genre = genre
            };

            return book;
        }
    }
}
