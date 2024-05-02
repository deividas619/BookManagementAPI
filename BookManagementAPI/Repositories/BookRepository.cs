using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookManagementAPI.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        try
        {
            var output = await context.Books.Include(b => b.Genre).Include(b => b.Reviews).ToListAsync();

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetAllBooks)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto filter, int skip, int take)
    {
        try
        {
            if (take <= 0)
                take = context.Books.Count();

            var result = context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                result = result.Where(b => b.Title.ToLower().Contains(filter.Title.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Author))
                result = result.Where(b => b.Author.ToLower().Contains(filter.Author.ToLower()));

            if (filter.Genres is { Length: > 0 })
                result = result.Where(b => filter.Genres.Contains(b.Genre.Name));

            if (filter.PublicationAfterDate != null)
                result = result.Where(b => b.Publication >= filter.PublicationAfterDate);

            if (filter.PublicationBeforeDate != null)
                result = result.Where(b => b.Publication <= filter.PublicationBeforeDate);

            var matchedBooks = await result.ToListAsync();
            foreach (var book in matchedBooks)
            {
                book.SearchedTimes++;
            }

            await context.SaveChangesAsync();

            var matchedBookIds = matchedBooks.Select(b => b.Id);
            result = context.Books.AsQueryable().Where(b => matchedBookIds.Contains(b.Id));

            result = result.Distinct()
                .OrderBy(b => b.Author)
                .ThenBy(b => b.Title)
                .Skip(skip)
                .Take(take);

            Log.Information($"[{nameof(GetBooksByFilter)}]: Successfully returned books by filter:\n" +
                            $"Title -> '{filter.Title}';\n" +
                            $"Author -> '{filter.Author}';\n" +
                            $"Genres -> '{filter.Genres}';\n" +
                            $"PublicationFromDate -> '{filter.PublicationAfterDate}';\n" +
                            $"PublicationToDate -> '{filter.PublicationBeforeDate}';\n" +
                            $"Skip -> '{skip}';\n" +
                            $"Take -> '{take}'");

            var output = await result.ToListAsync();

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBooksByFilter)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBookSuggestions(Guid bookId)
    {
        try
        {
            var filteredBook = await GetBookById(bookId);

            if (filteredBook == null)
            {
                return null;
            }

            var similarBooks = GetBooksByGenre(filteredBook.Genre.Id);

            similarBooks = similarBooks.OrderByDescending(b => b.SearchedTimes).ToList();

            similarBooks = similarBooks.Where(b => b.Id != bookId).ToList();

            var suggestions = similarBooks.Take(3).ToList();

            if (suggestions.Count < 3)
            {
                suggestions.Add(filteredBook);
            }

            return suggestions;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBookById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> GetBookById(Guid id)
    {
        try
        {
            var output = await context.Books.Include(g => g.Genre).FirstOrDefaultAsync(b => b.Id == id);

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBookById)}]: {e.Message}");
            throw;
        }
    }

    public IEnumerable<Book> GetBooksByGenre(Guid genreId)
    {
        return context.Books.Where(b => b.Genre.Id == genreId);
    }

    public async Task<Book> AddBook(Book book)
    {
        try
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddBook)}]: {e.Message}");
            throw;
        }

        Log.Information($"[{nameof(AddBook)}]: Successfully added book: {book.Id}");

        return book;
    }

    public async Task<Book> UpdateBook(Book currentBook)
    {
        try
        {
            context.Update(currentBook);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(UpdateBook)}]: {e.Message}");
            throw;
        }

        Log.Information($"[{nameof(UpdateBook)}]: Successfully updated book with id: {currentBook.Id}");

        return currentBook;
    }

    public async Task<Book> RemoveBookById(Guid bookId)
    {
        var bookToRemove = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        try
        {
            context.Books.Remove(bookToRemove);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
            throw;
        }
        
        Log.Information($"[{nameof(RemoveBookById)}]: Successfully removed book with id: {bookId}");

        return bookToRemove;
    }

    public async Task<Genre> GetGenre(GenreDto genreDto)
    {
        try
        {
            return await context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name) ?? new Genre { Id = Guid.NewGuid(), Name = genreDto.Name };
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetGenre)}]: {e.Message}");
            throw;
        }
    }

    public Guid GetUserId(string username)
    {
        return context.Users.SingleOrDefault(x => x.Username == username).Id;
    }
}