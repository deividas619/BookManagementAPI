using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookManagementAPI.Services.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        try
        {
            var output = await _context.Books.Include(g => g.Genre).ToListAsync();

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
            var output = await _context.Books.Include(g => g.Genre).Where(b => b.Title == title).ToListAsync();

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
            var output = await _context.Books.Include(g => g.Genre).FirstOrDefaultAsync(b => b.Id == id);

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
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

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
            Log.Error($"[{nameof(UpdateBook)}]: Could not find the book in the database by id!");
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
                Genre = currentBook.Genre
            };
            _context.Update(newBook);
            await _context.SaveChangesAsync();

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
        var book = await _context.Books.FindAsync(id);
        if (book is null)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: Could not find the book in the database by id: {id}!");
            return book;
        }

        try
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            Log.Information($"[{nameof(RemoveBookById)}]: Removed book by id: {id}!");

            return book;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto filter, int skip, int take)
    {
        try
        {
            if (take <= 0)
                take = 20;

            var result = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                result = result.Where(b => b.Title.Contains(filter.Title, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                result = result.Where(
                    b => b.Author.Contains(filter.Author, StringComparison.InvariantCultureIgnoreCase));

            if (filter.Genres is { Length: > 0 })
                result = result.Where(b => filter.Genres.Contains(b.Genre.Name));

            if (filter.PublicationAfterDate != null)
                result = result.Where(b => b.Publication >= filter.PublicationAfterDate);

            if (filter.PublicationBeforeDate != null)
                result = result.Where(b => b.Publication <= filter.PublicationBeforeDate);

            result = result.Distinct()
                .OrderBy(b => b.Author)
                .ThenBy(b => b.Title)
                .Skip(skip)
                .Take(take);

            Log.Information($"[{nameof(GetBooksByFilter)}]: Returned books by filter:" +
                            $"Title -> {filter.Title};" +
                            $"Author -> {filter.Author};" +
                            $"Genres -> {filter.Genres};" +
                            $"PublicationFromDate -> {filter.PublicationAfterDate};" +
                            $"PublicationToDate -> {filter.PublicationBeforeDate};" +
                            $"Skip -> {skip};" +
                            $"Take -> {take}!");

            var output = await result.ToListAsync();

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBooksByFilter)}]: {e.Message}");
            throw;
        }
    }
}