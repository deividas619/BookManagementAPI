using BookManagementAPI.Models;
using System.Collections.Generic;

namespace BookManagementAPI.DTOs
{
    public class ReviewWithAverageDto
    {
        public IEnumerable<Review> Reviews { get; set; }
        public double AverageRating { get; set; }
    }
}