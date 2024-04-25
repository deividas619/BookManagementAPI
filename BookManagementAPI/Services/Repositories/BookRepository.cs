using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookManagementAPI.Services.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        try
        {
            var output = await context.Books.Include(g => g.Genre).ToListAsync();

            Log.Information($"[{nameof(GetAllBooks)}]: Returned all books!");

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetAllBooks)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByTitle(string title)
    {
        try
        {
            var output = await context.Books.Include(g => g.Genre).Where(b => b.Title == title).ToListAsync();

            Log.Information($"[{nameof(GetBookById)}]: Returned books by title: {title}!");

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBooksByTitle)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> GetBookById(Guid id)
    {
        try
        {
            var output = await context.Books.Include(g => g.Genre).FirstOrDefaultAsync(b => b.Id == id);

            Log.Information($"[{nameof(GetBookById)}]: Returned book by id: {id}!");

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBookById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> AddBook(Book book)
    {
        try
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();

            Log.Information($"[{nameof(AddBook)}]: Added book: {book.Id}!");

            return book;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddBook)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> UpdateBook(Book currentBook)
    {
        if (currentBook is null)
        {
            Log.Error($"[{nameof(UpdateBook)}]: Could not find the book in the database by id: {currentBook.Id}!");
            return currentBook;
        }

        try
        {
            var newBook = new Book
            {
                Id = currentBook.Id,
                Author = currentBook.Author,
                Title = currentBook.Title,
                Publication = currentBook.Publication,
                Genre = currentBook.Genre,
                CreatedByUserId = currentBook.CreatedByUserId
            };
            context.Update(newBook);
            await context.SaveChangesAsync();

            Log.Information($"[{nameof(UpdateBook)}]: Updated book with id: {currentBook.Id}!");

            return newBook;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(UpdateBook)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> RemoveBookById(Guid id)
    {
        var book = await context.Books.FindAsync(id);
        if (book is null)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: Could not find the book in the database by id: {id}!");
            return book;
        }

        try
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();

            Log.Information($"[{nameof(RemoveBookById)}]: Removed book by id: {id}!");

            return book;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Genre> GetGenre(GenreDto genreDto)
    {
        try
        {
            return await context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name) ??
                   new Genre { Id = Guid.NewGuid(), Name = genreDto.Name };
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetGenre)}]: {e.Message}");
            throw;
        }
        public Guid GetUserId(string username)
        {
            return _context.Users.SingleOrDefault(x => x.Username == username).Id;
        }
    }
}