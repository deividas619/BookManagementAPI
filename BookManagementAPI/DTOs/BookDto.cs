using System;

namespace BookManagementAPI.DTOs;

public class BookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateOnly Publication { get; set; }
    public GenreDto Genre { get; set; }
}