using System;

namespace BookManagementAPI.DTOs;

public class SearchFilterDto
{
	public string? Title { get; set; }
	public string? Author { get; set; }
	public string[] Genres { get; set; }
	public DateOnly? PublicationFromDate { get; set; }
	public DateOnly? PublicationToDate { get; set; }
}
